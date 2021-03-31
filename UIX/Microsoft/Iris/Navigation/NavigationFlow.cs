// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Navigation.NavigationFlow
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Drawing;
using Microsoft.Iris.Session;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Iris.Navigation
{
    internal class NavigationFlow : NavigationItem
    {
        private NavigationOrientation _orientationValue;
        private NavigationPolicies _modeForNewSites;
        private bool _alignedSearchFlag;

        internal NavigationFlow(
          INavigationSite subjectSite,
          Direction searchDirection,
          NavigationOrientation orientionValue)
          : base(subjectSite, searchDirection)
        {
            this._orientationValue = orientionValue;
            this._modeForNewSites = NavigationPolicies.None;
            switch (this._orientationValue)
            {
                case NavigationOrientation.FlowHorizontal:
                    this._modeForNewSites = NavigationPolicies.Row;
                    break;
                case NavigationOrientation.FlowVertical:
                    this._modeForNewSites = NavigationPolicies.Column;
                    break;
            }
            this._alignedSearchFlag = this.SearchOrientation == this._orientationValue;
        }

        protected override IList ComputeSearchOrder(
          IList allChildrenList,
          RectangleF startRectangleF,
          bool enteringFlag)
        {
            List<NavigationItem> navigationItemList = null;
            Map<float, List<INavigationSite>> partitions = this.PartitionChildren(allChildrenList);
            if (partitions.Keys.Count > 0)
            {
                float[] keys = this.FilterPartitions(partitions, startRectangleF, enteringFlag);
                if (keys != null)
                {
                    this.SortPartitions(keys, startRectangleF);
                    navigationItemList = new List<NavigationItem>(partitions.Keys.Count);
                    foreach (float key in keys)
                    {
                        NavigationItem areaForPartition = this.CreateAreaForPartition(partitions[key]);
                        if (areaForPartition != null)
                            navigationItemList.Add(areaForPartition);
                    }
                }
            }
            return navigationItemList;
        }

        private Map<float, List<INavigationSite>> PartitionChildren(
          IList allChildrenList)
        {
            Map<float, List<INavigationSite>> map = new Map<float, List<INavigationSite>>();
            foreach (NavigationItem allChildren in allChildrenList)
            {
                float key = 0.0f;
                switch (this._orientationValue)
                {
                    case NavigationOrientation.FlowHorizontal:
                        key = allChildren.Position.Y;
                        break;
                    case NavigationOrientation.FlowVertical:
                        key = allChildren.Position.X;
                        break;
                }
                if (!map.ContainsKey(key))
                    map[key] = new List<INavigationSite>();
                map[key].Add(allChildren.Subject);
            }
            return map;
        }

        private float[] FilterPartitions(
          Map<float, List<INavigationSite>> partitions,
          RectangleF startRectangleF,
          bool enteringFlag)
        {
            Vector<float> vector = new Vector<float>(!this._alignedSearchFlag || enteringFlag ? partitions.Keys.Count : 1);
            foreach (float key in partitions.Keys)
            {
                if (this.PartitionIsPotentialCandidate(key, startRectangleF, enteringFlag))
                {
                    vector.Add(key);
                    if (this._alignedSearchFlag && !enteringFlag)
                        break;
                }
            }
            return vector.Count <= 0 ? null : vector.ToArray();
        }

        private bool PartitionIsPotentialCandidate(
          float key,
          RectangleF startRectangleF,
          bool enteringFlag)
        {
            if (this._alignedSearchFlag)
            {
                if (enteringFlag)
                    return true;
                float num1 = 0.0f;
                float num2 = 0.0f;
                switch (this._orientationValue)
                {
                    case NavigationOrientation.FlowHorizontal:
                        num1 = startRectangleF.Top;
                        num2 = startRectangleF.Bottom;
                        break;
                    case NavigationOrientation.FlowVertical:
                        num1 = startRectangleF.Left;
                        num2 = startRectangleF.Right;
                        break;
                }
                return num1 <= (double)key && key <= (double)num2;
            }
            switch (this.SearchDirection)
            {
                case Direction.North:
                    return key < (double)startRectangleF.Top;
                case Direction.South:
                    return key > (double)startRectangleF.Bottom;
                case Direction.East:
                    return key > (double)startRectangleF.Right;
                case Direction.West:
                    return key < (double)startRectangleF.Left;
                default:
                    return false;
            }
        }

        private void SortPartitions(float[] keys, RectangleF startRectangleF)
        {
            float originValue = 0.0f;
            switch (this._orientationValue)
            {
                case NavigationOrientation.FlowHorizontal:
                    originValue = startRectangleF.Center.Y;
                    break;
                case NavigationOrientation.FlowVertical:
                    originValue = startRectangleF.Center.X;
                    break;
            }
            Array.Sort<float>(keys, new NavigationFlow.CompareItemDistance(originValue));
        }

        private NavigationItem CreateAreaForPartition(List<INavigationSite> partition)
        {
            NavigationItem navigationItem = null;
            if (partition != null && partition.Count > 0)
                navigationItem = NavigationItem.CreateAreaForSite(new TransientNavigationSite(partition[0].ToString(), this.Subject, partition, this._modeForNewSites, Vector3.Zero, Vector3.Zero), this.SearchDirection, false, true);
            return navigationItem;
        }

        private NavigationOrientation SearchOrientation
        {
            get
            {
                switch (this.SearchDirection)
                {
                    case Direction.North:
                    case Direction.South:
                        return NavigationOrientation.FlowVertical;
                    case Direction.East:
                    case Direction.West:
                        return NavigationOrientation.FlowHorizontal;
                    default:
                        return this._orientationValue;
                }
            }
        }

        private class CompareItemDistance : IComparer<float>
        {
            private float _originValue;

            public CompareItemDistance(float originValue) => this._originValue = originValue;

            int IComparer<float>.Compare(float leftValue, float rightValue)
            {
                leftValue -= this._originValue;
                if (leftValue < 0.0)
                    leftValue *= -1f;
                rightValue -= this._originValue;
                if (rightValue < 0.0)
                    rightValue *= -1f;
                if (leftValue < (double)rightValue)
                    return -1;
                return rightValue < (double)leftValue ? 1 : 0;
            }
        }
    }
}
