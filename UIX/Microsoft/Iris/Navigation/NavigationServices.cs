﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Navigation.NavigationServices
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Debug;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Drawing;
using Microsoft.Iris.Session;
using System.Collections;
using System.Diagnostics;

namespace Microsoft.Iris.Navigation
{
    internal static class NavigationServices
    {
        private static float s_originNear = 0.0f;
        private static float s_originSize = 0.0f;
        private static NavigationServices.SearchOrientation s_originOrientation = NavigationServices.SearchOrientation.None;

        public static bool FindNextPeer(
          INavigationSite originSite,
          Direction searchDirection,
          RectangleF startRectangleF,
          out INavigationSite resultSite)
        {
            INavigationSite navigationSite1 = originSite;
            Microsoft.Iris.Debug.Trace.IsCategoryEnabled(TraceCategory.Navigation, (byte)2);
            if (startRectangleF.IsEmpty && !NavigationServices.GetDefaultOutboundStartRect(originSite, out startRectangleF))
            {
                resultSite = (INavigationSite)null;
                return false;
            }
            NavigationServices.ProcessDirectionalMemory(ref startRectangleF, searchDirection);
            INavigationSite parentTabGroup = NavigationServices.FindParentTabGroup(navigationSite1, searchDirection);
            if (parentTabGroup != null)
                navigationSite1 = parentTabGroup;
            INavigationSite boundingSite = NavigationServices.FindBoundingSite(navigationSite1, searchDirection);
            INavigationSite navigationSite2 = (INavigationSite)null;
            NavigationItem itemForSite1 = NavigationItem.CreateItemForSite(navigationSite1, searchDirection, false);
            if (itemForSite1 != (NavigationItem)null)
            {
                NavigationItem navigationItem1 = itemForSite1.SearchUpTree(startRectangleF, (INavigationSite)null, (INavigationSite)null);
                if (navigationItem1 != (NavigationItem)null)
                    navigationSite2 = navigationItem1.Subject;
                if (navigationSite2 == null && boundingSite != null && NavigationItem.IsWrappingSite(boundingSite, searchDirection))
                {
                    Vector3 positionPxlVector;
                    Vector3 sizePxlVector;
                    boundingSite.ComputeBounds(out positionPxlVector, out sizePxlVector);
                    RectangleF excludeRectangleF = new RectangleF(positionPxlVector.X, positionPxlVector.Y, sizePxlVector.X, sizePxlVector.Y);
                    NavigationServices.AdjustStartRectForSimulatedEntry(searchDirection, excludeRectangleF, ref startRectangleF);
                    NavigationItem itemForSite2 = NavigationItem.CreateItemForSite(boundingSite, searchDirection, true);
                    if (itemForSite2 != (NavigationItem)null)
                    {
                        NavigationItem navigationItem2 = itemForSite2.SearchDownTree(startRectangleF, true, boundingSite, originSite);
                        if (navigationItem2 != (NavigationItem)null)
                            navigationSite2 = navigationItem2.Subject;
                    }
                    if (navigationSite2 == originSite)
                        navigationSite2 = (INavigationSite)null;
                }
            }
            bool flag = navigationSite2 != null;
            resultSite = navigationSite2;
            return flag || boundingSite != null;
        }

        public static bool FindFromPoint(
          INavigationSite originSite,
          PointF pt,
          out INavigationSite result)
        {
            return NavigationServices.FindFromPoint(originSite, Direction.Next, pt, out result);
        }

        public static bool FindFromPoint(
          INavigationSite originSite,
          Direction bias,
          PointF pt,
          out INavigationSite result)
        {
            NavigationServices.ResetDirectionalMemory();
            return new FindFromPointWorker(originSite, bias).FindFromPoint(pt, out result);
        }

        public static bool FindNextWithin(
          INavigationSite originSite,
          Direction searchDirection,
          RectangleF startRectangleF,
          out INavigationSite resultSite)
        {
            Microsoft.Iris.Debug.Trace.IsCategoryEnabled(TraceCategory.Navigation, (byte)2);
            if (startRectangleF.IsEmpty && !NavigationServices.GetDefaultInboundStartRect(originSite, searchDirection, out startRectangleF))
            {
                resultSite = (INavigationSite)null;
                return false;
            }
            NavigationServices.ProcessDirectionalMemory(ref startRectangleF, searchDirection);
            INavigationSite navigationSite = (INavigationSite)null;
            NavigationItem itemForSite = NavigationItem.CreateItemForSite(originSite, searchDirection, true);
            if (itemForSite != (NavigationItem)null)
            {
                NavigationItem navigationItem = itemForSite.SearchDownTree(startRectangleF, true, (INavigationSite)null, (INavigationSite)null);
                if (navigationItem != (NavigationItem)null)
                    navigationSite = navigationItem.Subject;
            }
            bool flag = navigationSite != null;
            resultSite = navigationSite;
            return flag;
        }

        public static void SeedDefaultFocus(INavigationSite focusSite)
        {
            if (focusSite == null)
                return;
            NavigationItem.RememberFocus(focusSite);
        }

        public static void ClearDefaultFocus(INavigationSite startSite) => NavigationItem.ClearFocus(startSite);

        public static bool GetDefaultInboundStartRect(
          INavigationSite originSite,
          Direction searchDirection,
          out RectangleF startRectangleF)
        {
            if (!NavigationServices.GetDefaultOutboundStartRect(originSite, out startRectangleF))
                return false;
            NavigationServices.AdjustStartRectForSimulatedEntry(searchDirection, startRectangleF, ref startRectangleF);
            return true;
        }

        public static bool GetDefaultOutboundStartRect(
          INavigationSite originSite,
          out RectangleF startRectangleF)
        {
            if (!originSite.Visible)
            {
                startRectangleF = RectangleF.Zero;
                return false;
            }
            Vector3 positionPxlVector;
            Vector3 sizePxlVector;
            originSite.ComputeBounds(out positionPxlVector, out sizePxlVector);
            startRectangleF = new RectangleF(positionPxlVector.X, positionPxlVector.Y, sizePxlVector.X, sizePxlVector.Y);
            return true;
        }

        private static void ResetDirectionalMemory() => NavigationServices.UpdateDirectionalMemoryInfo(RectangleF.Zero, Direction.Next);

        private static bool UpdateDirectionalMemoryInfo(
          RectangleF startRectangleF,
          Direction searchDirection)
        {
            float num1 = 0.0f;
            float num2 = 0.0f;
            NavigationServices.SearchOrientation searchOrientation = NavigationServices.SearchOrientation.None;
            switch (searchDirection)
            {
                case Direction.North:
                case Direction.South:
                    searchOrientation = NavigationServices.SearchOrientation.Vertical;
                    num1 = startRectangleF.X;
                    num2 = startRectangleF.Width;
                    break;
                case Direction.East:
                case Direction.West:
                    searchOrientation = NavigationServices.SearchOrientation.Horizontal;
                    num1 = startRectangleF.Y;
                    num2 = startRectangleF.Height;
                    break;
                case Direction.Previous:
                case Direction.Next:
                    searchOrientation = NavigationServices.SearchOrientation.None;
                    break;
            }
            bool flag = NavigationServices.s_originOrientation != searchOrientation;
            if (flag)
            {
                NavigationServices.s_originOrientation = searchOrientation;
                NavigationServices.s_originNear = num1;
                NavigationServices.s_originSize = num2;
            }
            return flag;
        }

        private static void ProcessDirectionalMemory(
          ref RectangleF startRectangleF,
          Direction searchDirection)
        {
            if (NavigationServices.UpdateDirectionalMemoryInfo(startRectangleF, searchDirection))
                return;
            bool flag = true;
            switch (NavigationServices.s_originOrientation)
            {
                case NavigationServices.SearchOrientation.Horizontal:
                    startRectangleF.Y = NavigationServices.s_originNear;
                    startRectangleF.Height = NavigationServices.s_originSize;
                    break;
                case NavigationServices.SearchOrientation.Vertical:
                    startRectangleF.X = NavigationServices.s_originNear;
                    startRectangleF.Width = NavigationServices.s_originSize;
                    break;
                case NavigationServices.SearchOrientation.None:
                    flag = false;
                    break;
            }
            int num = flag ? 1 : 0;
        }

        private static INavigationSite FindBoundingSite(
          INavigationSite searchSite,
          Direction searchDirection)
        {
            while (searchSite != null && !NavigationItem.IsBoundingSite(searchSite, searchDirection))
                searchSite = searchSite.Parent;
            return searchSite;
        }

        private static INavigationSite FindParentTabGroup(
          INavigationSite targetSite,
          Direction searchDirection)
        {
            switch (searchDirection)
            {
                case Direction.Previous:
                case Direction.Next:
                    for (; targetSite != null; targetSite = targetSite.Parent)
                    {
                        if (NavigationItem.IsBoundingSite(targetSite, searchDirection))
                            return (INavigationSite)null;
                        if (NavigationItem.IsTabGroup(targetSite))
                            break;
                    }
                    return targetSite;
                default:
                    return (INavigationSite)null;
            }
        }

        private static void AdjustStartRectForSimulatedEntry(
          Direction searchDirection,
          RectangleF excludeRectangleF,
          ref RectangleF startRectangleF)
        {
            switch (searchDirection)
            {
                case Direction.North:
                    startRectangleF.Y = excludeRectangleF.Bottom;
                    break;
                case Direction.South:
                    startRectangleF.Y = excludeRectangleF.Top - startRectangleF.Height;
                    break;
                case Direction.East:
                    startRectangleF.X = excludeRectangleF.Left - startRectangleF.Width;
                    break;
                case Direction.West:
                    startRectangleF.X = excludeRectangleF.Right;
                    break;
            }
        }

        [Conditional("DEBUG")]
        private static void DumpSiteTree(INavigationSite branchSite, INavigationSite markSite)
        {
            int num = branchSite.IsLogicalJunction ? 1 : 0;
            if (branchSite.Navigability != NavigationClass.None)
                InvariantString.Format(", Class: {0}", (object)branchSite.Navigability);
            if (branchSite.Mode != NavigationPolicies.None)
                InvariantString.Format(", Mode: {0}", (object)branchSite.Mode);
            foreach (INavigationSite child in (IEnumerable)branchSite.Children)
                ;
        }

        [Conditional("DEBUG")]
        private static void DumpSiteTreeContaining(INavigationSite originSite)
        {
            INavigationSite navigationSite = originSite;
            while (true)
            {
                INavigationSite parent = navigationSite.Parent;
                if (parent != null)
                    navigationSite = parent;
                else
                    break;
            }
        }

        private enum SearchOrientation
        {
            Horizontal,
            Vertical,
            None,
        }
    }
}