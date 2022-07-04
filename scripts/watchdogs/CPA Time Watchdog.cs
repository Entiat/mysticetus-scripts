// ======================================
// CPA Bounds Watchdog
// ======================================
// MIT License
//
// Copyright 2022 Mysticetus, LLC
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
// =======================================
//
// This script runs as a watchdog to check that the value in one time field (Time at CPA to Active Source) 
// is between two other time fields (Time at first detection, Time at last detection).
//
// Recommended Settings:
//      Repeat Interval: 30 seconds
//      Run Mode: Both (collection for PSOs, editing for PMs)
//


// Field and sheet names
var detectionSheetName = "4 - RPS Visual Detection";
var cpaTimeField = "Sighting time at CPA to Active Source";
var firstTimeDetectionField = "Time at first detection";
var lastTimeDetectionField = "Time at last detection";

// Get the ids for each row in the detection sheet
var rowIds = Data.GetRowIds(detectionSheetName);

// Scan each row, looking for out of bounds data
foreach (var rowId in rowIds)
{
    // Check to see if we've been told to stop processing (app is exiting, or user pressed Cancel)
    if (cancel.IsCancellationRequested)
        return;

    // Time stamps return floating point unix time (seconds since jan 1, 1970). Some of these times in this particular
    // schema are stored as strings (instead of DateTime values). Use the ToTime function to convert.
    if (Data.ToTime(Data.GetData(detectionSheetName, rowId, cpaTimeField)) is double cpaTime && !double.IsNaN(cpaTime) &&
        Data.ToTime(Data.GetData(detectionSheetName, rowId, firstTimeDetectionField)) is double firstTime && !double.IsNaN(firstTime) &&
        Data.ToTime(Data.GetData(detectionSheetName, rowId, lastTimeDetectionField)) is double lastTime && !double.IsNaN(cpaTime))
    {
        // Check that cpa is between first and last detection times
        if (cpaTime < firstTime || cpaTime > lastTime)
        {
            // Fire a watchdog. Will display as "Concern" in the status panel. User can click to go to the offending field.
            Watchdogs.BarkAtSheetData(
                watchdogName: "CPA Time", 
                watchdogDescription: "Checks whether CPA time is between first and last detection times", 
                error: $"'{cpaTimeField}' is outside of '{firstTimeDetectionField}' and '{lastTimeDetectionField}'.", 
                sheet: detectionSheetName, 
                rowId: rowId, 
                field: cpaTimeField);

            return; // stop looking after first error
        }
    }
}

// If we made it here, we found no errors. Clear any previous cpa watchdog status.
Watchdogs.Clear("CPA Time");
