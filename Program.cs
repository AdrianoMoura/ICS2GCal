// Program.cs
// Reads a .ics, picks the first VEVENT, and opens Google Calendar prefilled.

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using IcalCalendar = Ical.Net.Calendar;          
using Ical.Net.CalendarComponents;               
using Ical.Net.DataTypes;                        


static class UI
{
    // Win32 MessageBoxW
    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int MessageBoxW(IntPtr hWnd, string text, string caption, uint type);

    private const uint MB_OK           = 0x00000000;
    private const uint MB_ICONERROR    = 0x00000010;
    private const uint MB_ICONINFO     = 0x00000040;
    private const uint MB_TOPMOST      = 0x00040000;
    private const uint MB_SETFOREGROUND= 0x00010000;

    public static void ShowError(string message, string title = "ICS2GCal - Error")
        => MessageBoxW(IntPtr.Zero, message, title, MB_OK | MB_ICONERROR | MB_TOPMOST | MB_SETFOREGROUND);

    public static void ShowInfo(string message, string title = "ICS2GCal")
        => MessageBoxW(IntPtr.Zero, message, title, MB_OK | MB_ICONINFO | MB_TOPMOST | MB_SETFOREGROUND);
}

class Program
{
    static string UrlEncode(string s) =>
        Uri.EscapeDataString(s ?? string.Empty);

    static string BuildQuery(IDictionary<string, string?> kv)
    {
        var sb = new StringBuilder();
        bool first = true;
        foreach (var kvp in kv)
        {
            if (kvp.Value is null) continue;
            if (!first) sb.Append('&');
            first = false;
            sb.Append(UrlEncode(kvp.Key));
            sb.Append('=');
            sb.Append(UrlEncode(kvp.Value));
        }
        return sb.ToString();
    }

    static string FormatDates(DateTime startUtc, DateTime endUtc, bool allDay)
    {
        if (allDay)
            return $"{startUtc:yyyyMMdd}/{endUtc:yyyyMMdd}";
        return $"{startUtc:yyyyMMdd'T'HHmmss'Z'}/{endUtc:yyyyMMdd'T'HHmmss'Z'}";
    }

    static void OpenUrl(string url)
    {
        var psi = new ProcessStartInfo { FileName = url, UseShellExecute = true };
        Process.Start(psi);
    }

    static CalendarEvent? FirstEventFromIcs(string icsText)
    {
        var cal = IcalCalendar.Load(icsText);
        foreach (var e in cal.Events) return e; // first VEVENT
        return null;
    }

    static string ReadIcs(string pathOrUrl)
    {
        if (pathOrUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            pathOrUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            using var http = new HttpClient();
            return http.GetStringAsync(pathOrUrl).GetAwaiter().GetResult();
        }
        return Encoding.UTF8.GetString(File.ReadAllBytes(pathOrUrl));
    }

    static DateTime ToUtc(CalDateTime dt)
    {
        if (dt == null) throw new InvalidOperationException("Missing datetime");
        var val = dt.Value;
        return val.Kind == DateTimeKind.Utc ? val : val.ToUniversalTime();
    }

    static void Main(string[] args)
    {
        try
        {
            if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
            {
                UI.ShowError(
                    "No .ics file was provided.\n\n" +
                    "How to use:\n" +
                    "• Right-click an .ics → Open with → ICS2GCal (check Always), or\n" +
                    "• Set ICS2GCal as the default app for .ics in Windows Settings."
                );
                return;
            }

            var ics = ReadIcs(args[0]);
            var evt = FirstEventFromIcs(ics);
            if (evt == null)
            {
                UI.ShowError("This .ics file contains no VEVENT.");
                return;
            }

            var title = evt.Summary ?? "Untitled event";
            var location = evt.Location ?? "";
            var details = evt.Description ?? "";

            var dtStart = evt.DtStart; // CalDateTime
            var dtEnd = evt.DtEnd;     // CalDateTime or null

            if (dtStart == null)
            {
                UI.ShowError("This .ics is missing DTSTART (start date/time).");
                return;
            }

            bool isAllDay = !dtStart.HasTime;
            DateTime startUtc, endUtc;

            if (isAllDay)
            {
                // Use the date component from Value (DateTime) to avoid DateOnly issues
                DateTime startDate = dtStart.Value.Date;
                DateTime endDate = (dtEnd != null ? dtEnd.Value.Date : startDate.AddDays(1)); // ICS end is exclusive

                // For all-day, Google expects plain dates (no TZ). We'll pass them as local dates.
                startUtc = startDate;
                endUtc = endDate;
            }
            else
            {
                startUtc = ToUtc(dtStart);
                if (dtEnd != null && dtEnd.HasTime)
                    endUtc = ToUtc(dtEnd);
                else
                    endUtc = startUtc.AddHours(1); // fallback if no DTEND/DURATION
            }

            var dates = FormatDates(startUtc, endUtc, isAllDay);

            var query = BuildQuery(new Dictionary<string, string?>
            {
                ["action"] = "TEMPLATE",
                ["text"] = title,
                ["dates"] = dates,
                ["details"] = string.IsNullOrWhiteSpace(details) ? null : details,
                ["location"] = string.IsNullOrWhiteSpace(location) ? null : location
            });

            var url = "https://calendar.google.com/calendar/render?" + query;
            Console.WriteLine("Opening: " + url);
            OpenUrl(url);
        }
        catch (Exception ex)
        {
            UI.ShowError("Unexpected error.\n\n" + ex.Message);
        }
    }
}
