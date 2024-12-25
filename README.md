# mysticetus-scripts

This repo contains scripts for use in the Mysticetus app. Mysticetus introduced the ability to run user-created C# scripts in build 2022.32 (July 2022).

All scripts hosted here live under the MIT license - you can use these for anything, anywhere, anytime, anyhow. All the disclaimers in the license apply, of course. These scripts are not guaranteed or warrantied to be useful for anything. Like all MIT-licensed software, they may have bugs, you may lose data, they may crash apps or crash your computer, they may cause heartbreak and you may need therapy, and that's all on you. Use at your own risk.

# Script Submission

Submit a PR with your script. It must contain the MIT License in the header as a C# comment. A template is provided below. By submitting your script you are agreeing to the license - anyone at all can use your script for anything, modify it, resell it, yell at it, write a song about it and earn royalties from record companies - whatever they want (as long as they preserve the MIT license with it).

Feel free to include additional comments about usage, gotchas, data schemas this works with, etc.

Do put the year and copyright holder in there. DO NOT put identifiable info (email, phone, etc.) in there. This is a public repo no doubt scanned by zillions of data harvester bots.

# MIT License Template
```csharp
    // MIT License
    //
    // Copyright <YEAR> <COPYRIGHT HOLDER>
    //
    // Permission is hereby granted, free of charge, to any person obtaining a copy of this
    // software and associated documentation files (the "Software"), to deal in the Software
    // without restriction, including without limitation the rights to use, copy, modify,
    // merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
    // permit persons to whom the Software is furnished to do so.
    // 
    // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
    // INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
    // PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
    // HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
    // OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
    // SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    //
```
# Script Support
Currently, the following functions are available for scripts. We will be adding more functionality over time. Let us know what features you'd like to have access to or if we can improve the current API in any way.
```csharp
#region Data
/// <summary>
/// Script support for Data items.
/// </summary>
public static class Data
{
    /// <summary>
    /// Adds a row to the specified sheet and returns the new row's id.
    /// </summary>
    /// <param name="sheet">Sheet to add to.</param>
    /// <param name="copyPrevRow">Whether or not to copy the previous row (if it exists) to this new row</param>
    /// <returns>Row id of the new row, or Guid.Empty on error (sheet not found, etc.)</returns>
    public static Guid AddRow(string sheet, bool copyPrevRow);

    /// <summary>
    /// Determines whether or not a field in a sheet exists.
    /// </summary>
    /// <param name="sheet">Sheet to find.</param>
    /// <param name="field">Field in the sheet to find.</param>
    /// <returns>True if sheet exists and contains field named 'field'.</returns>
    public static bool FieldExists(string sheet, string field);

    /// <summary>
    /// Retrieves data from a field. 
    /// </summary>
    /// <param name="sheet">Sheet to access</param>
    /// <param name="rowId">Row number to access.</param>
    /// <param name="field">Field name to retrieve</param>
    /// <returns>Data, or null on error (or empty field). Times are returned as a unix time unix times (seconds since jan 1, 1970) double value inc fractional seconds.</returns>
    public static object? GetData(string sheet, Guid rowId, string field);

    /// <summary>
    /// Retrieves the list of row ids in the given sheet.
    /// </summary>
    /// <param name="sheet">Sheet name</param>
    /// <returns>RowIds of this sheet, or empty list on error.</returns>
    public static List<Guid> GetRowIds(string sheet);

    /// <summary>
    /// Sets a data value into a data field.
    /// </summary>
    /// <param name="sheet">Sheet to set a value into.</param>
    /// <param name="rowId">RowId to set the value into.</param>
    /// <param name="field">Field to set the value into.</param>
    /// <param name="data">Data to place in the row.</param>
    public static void SetData(string sheet, Guid rowId, string field, object? data);

    /// <summary>
    /// Gets whether or not a data sheet exists
    /// </summary>
    /// <param name="sheet">Sheet name</param>
    /// <returns>True if it exists, false if not.</returns>
    public static bool SheetExists(string sheet);

    /// <summary>
    /// Attempts to convert an object into a unix time (seconds since Jan 1, 1970). Will be negative if before 1970.
    /// Doubles are returned as-is. Ints are converted to double. Strings are parsed. Anything else returns double.NaN.
    /// </summary>
    /// <param name="time">Value to convert.</param>
    /// <returns>Seconds since Jan 1, 1970, or double.NaN if unable to parse/convert.</returns>
    public static double ToTime(object? time);
}
#endregion

#region UI
/// <summary>
/// Script support for UI 
/// </summary>
public static class UI
{
    /// <summary>
    /// Shows a form as a modal dialog.
    /// </summary>
    /// <param name="form">Form to display.</param>
    /// <returns>Result of the form.</returns>
    public static DialogResult ShowDialog(Form form);

    /// <summary>
    /// Shows a message box
    /// </summary>
    /// <param name="text">Text to display.</param>
    /// <param name="title">Title</param>
    /// <returns>Result of the MessageBox</returns>
    public static DialogResult MessageBox(string text, string title, MessageBoxButtons buttons);
}
#endregion

#region Config
public static class Config
{
    /// <summary>
    /// Retrieves a variable value, optionally for a given vehicle.
    /// </summary>
    /// <param name="variable">The variale value to retrieve</param>
    /// <param name="vehicle">If non-null, the variable associated with this vehicle. If null, looks for "all vehicles"</param>
    /// <returns>Variable value if found (optionally for specified vehicle) - null, otherwise </returns>
    public static string? GetVariableValue(string variable, string? vehicle = null);

    /// <summary>
    /// Retrieves the contents of the given named list, optionally for a specified vehicle.
    /// </summary>
    /// <param name="list">Named list to retrieve. Can include known lists such as {observers}, {obsplatforms}, etc.</param>
    /// <param name="vehicle">Optional vehicle to obtain list for. If null looks for "all vehicles"</param>
    /// <returns>List, optionally associated with this vehicle. Empty list on error.</returns>
    public static List<string> GetNamedList(string list, string? vehicle = null);

    /// <summary>
    /// Gets the name of the primary vehicle, or null if no primary vehicle.
    /// </summary>
    public static string? PrimaryVehicleName { get; }
}
#endregion

#region Watchdogs
/// <summary>
/// Script support for watchdogs.
/// </summary>
public static class Watchdogs
{
    /// <summary>
    /// Removes the watchdog status for this named watchdog
    /// </summary>
    /// <param name="watchdogName">Name of watchdog</param>
    public static void Clear(string watchdogName);

    /// <summary>
    /// Sets the named watchdog status to OK.
    /// </summary>
    /// <param name="watchdogName"></param>
    /// <param name="watchdogDescription"></param>
    public static void SetOk(string watchdogName, string watchdogDescription);

    /// <summary>
    /// Sets Watchdog to a "Concern" state about data in an entry sheet.
    /// </summary>
    /// <param name="watchdogName">Name of watchdog</param>
    /// <param name="watchdogDescription">Description of this watchdog</param>
    /// <param name="error">Error text displayed to user when they click on Concern.</param>
    public static void BarkAtSheetData(string watchdogName, string watchdogDescription, string error, string sheet, Guid rowId, string field);
    
    /// <summary>
    /// Gets the current error count for scripts and plugins
    /// </summary>
    public static int CurrentErrorCount { get; }
}
#endregion
```
