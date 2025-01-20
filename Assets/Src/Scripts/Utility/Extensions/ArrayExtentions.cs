using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ArrayExtensions {
    public static T[] Add<T>(this T[] array, T item) {
        int lenght = array != null ? array.Length : 0;
        T[] newArray = new T[lenght + 1];

        if (array != null) array.CopyTo(newArray, 0);
        newArray[lenght] = item;
        return newArray;
    }
    
    public static T[] RemoveAt<T>(this T[] that, int index) {
        T[] dest = new T[that.Length - 1];
        if (index > 0) {
            Array.Copy(that, 0, dest, 0, index);
        }
        if (index < that.Length - 1) {
            Array.Copy(that, index + 1, dest, index, that.Length - index - 1);
        }
        return dest;
    }

}
