using System;
using System.Collections.Generic;
using UnityEngine;

public static class CollectionExtensions {
    /// <summary>
    /// Get a random item from the collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="r"></param>
    /// <returns>A Random Item from the collection</returns>
    public static T GetRandom<T>(this ICollection<T> r) {
        List<T> list = new List<T>(r);

        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    /// <summary>
    /// Randomize the collection and returns the result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns>A new collection with the same items, of which the order was randomized</returns>
    public static ICollection<T> RandomSelf<T>(this ICollection<T> t) {
        System.Random rng = new System.Random();
        List<T> list = new List<T>(t);

        int rIndex = t.Count;
        while (rIndex > 1) {
            int currentShuffle = rng.Next(rIndex--);
            T temp = list[currentShuffle];
            list[currentShuffle] = list[rIndex];
            list[rIndex] = temp;
        }

        return list;
    }

    /// <summary>
    /// Returns a random item from the collection and removes the result from the collection.
    /// Useful if you have a list of candidates from which you want only candidates you don't want to get twice.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="r"></param>
    /// <returns>A random item from the collection, which has been removed</returns>
    public static T GetRandomAndRemove<T>(this ICollection<T> r) {
        List<T> list = new List<T>(r);

        T res = r.GetRandom();
        list.Remove(res);
        r = list;
        return res;
    }

    /// <summary>
    /// Get a collection of random items from collection without duplicates.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when param count is greater than collection size</exception>
    /// <typeparam name="T"></typeparam>
    /// <param name="r"></param>
    /// <param name="count">The amount of items wanted</param>
    /// <returns></returns>
    public static T[] GetRandomCountNoDuplicates<T>(this ICollection<T> r, int count) {
        List<T> candidates = new List<T>(r);

        if (count > candidates.Count)
            throw new InvalidOperationException($"Tried to get {count} items from a collections that only contains {candidates.Count}");

        T[] res = new T[count];
        for (int i = 0; i < count; i++) {
            res[i] = candidates.GetRandom();
            candidates.Remove(res[i]);
        }

        return res;
    }

    /// <summary>
    /// Get an array of random items from collection without excluding duplicates
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when param count is greater than collection size</exception>
    /// <typeparam name="T"></typeparam>
    /// <param name="r"></param>
    /// <param name="count">The amount of items wanted</param>
    /// <returns></returns>
    public static T[] GetRandom<T>(this ICollection<T> r, int count) {
        T[] res = new T[count];

        List<T> l = new List<T>(r);
        if (count > l.Count)
            throw new InvalidOperationException($"Tried to get {count} items from a collections that only contains {l.Count}");

        for (int i = 0; i < count; i++)
            res[i] = r.GetRandom();

        return res;
    }

    /// <summary>
    /// Remove items passed as params to the collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="r"></param>
    /// <param name="items">One or more items to be removed</param>
    public static void Remove<T>(this ICollection<T> r, params T[] items) {
        foreach (T item in items)
            if (r.Contains(item))
                r.Remove(item);
    }

    /// <summary>
    /// Creates a new collection containing elements of both collection passed as parameters
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="first">First collection</param>
    /// <param name="second">Second collection</param>
    /// <returns>A collection containing elements of both parameters</returns>
    public static ICollection<T> AddAndClone<T>(this ICollection<T> first, ICollection<T> second) {
        List<T> result = new List<T>(first);
        result.AddRange(second);
        return result;
    }

    /// <summary>
    /// Adds all the elements of the collections passed parameters to the list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target"></param>
    /// <param name="others">One or more collection of which the items should be added to the list</param>
    /// <returns>A reference to the modified list</returns>
    public static ICollection<T> AddRangeSeveral<T>(this ICollection<T> target, params ICollection<T>[] others) {
        foreach (ICollection<T> list in others)
            foreach (T other in list)
                target.Add(other);

        return target;
    }

    public static ICollection<T> RemoveRange<T>(this ICollection<T> target, params ICollection<T>[] others) {
        foreach (ICollection<T> list in others)
            foreach (T other in list)
                target.Remove(other);

        return target;
    }

    /// <summary>
    /// Removes all disabled monobehavior from the collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target"></param>
    public static void RemoveDisabled<T>(this ICollection<T> target) where T : MonoBehaviour {
        List<T> list = new List<T>(target);
        foreach (T t in list)
            if (t.enabled == false)
                target.Remove(t);
    }
}
