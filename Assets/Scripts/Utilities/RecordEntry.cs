using System;
using System.Collections;
using System.Collections.Generic;

public class RecordEntry : IComparable
{
    public string userName;
    public int collectedCoins;

    public RecordEntry(string userName, int collectedCoins)
    {
        this.userName = userName;
        this.collectedCoins = collectedCoins;
    }

    public static int CompareIfBigger(RecordEntry entry1, RecordEntry entry2)
    {
        if (entry1.collectedCoins == entry2.collectedCoins) {
            return 0;
        }
        else if (entry1.collectedCoins < entry2.collectedCoins) {
            return 1;
        }
        else {
            return -1;
        }
    }
    
    public int CompareTo(object obj)
    {
        RecordEntry entry1 = this;
        RecordEntry entry2 = obj as RecordEntry;
        if (entry1.collectedCoins == entry2.collectedCoins) {
            return 0;
        }
        else if (entry1.collectedCoins < entry2.collectedCoins) {
            return -1;
        }
        else {
            return 1;
        }
    }
}