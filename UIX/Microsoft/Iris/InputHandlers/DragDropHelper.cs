// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.InputHandlers.DragDropHelper
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Input;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.InputHandlers
{
    internal static class DragDropHelper
    {
        private static bool _draggingInternally;
        private static DragSourceHandler _sourceHandler;
        private static DropTargetHandler _targetHandler;
        private static DropAction _dropAction;

        public static bool DraggingInternally => DragDropHelper._draggingInternally;

        public static DragSourceHandler SourceHandler => DragDropHelper._sourceHandler;

        public static DropTargetHandler TargetHandler
        {
            get => DragDropHelper._targetHandler;
            set
            {
                if (DragDropHelper._targetHandler == value)
                    return;
                DropTargetHandler targetHandler = DragDropHelper._targetHandler;
                DragDropHelper._targetHandler = value;
                DragDropHelper.OnAllowedDropActionsChanged();
            }
        }

        public static DropAction AllowedDropActions => DragDropHelper._targetHandler == null ? DropAction.None : DragDropHelper._targetHandler.AllowedDropActions;

        public static InputModifiers Modifiers => UISession.Default.InputManager.DragModifiers;

        public static void OnAllowedDropActionsChanged()
        {
            if (DragDropHelper.DraggingInternally)
            {
                DragDropHelper._sourceHandler.UpdateCurrentAction();
            }
            else
            {
                uint allowedDropActions = (uint)DragDropHelper.AllowedDropActions;
                UISession.Default.Form.SetDragDropResult(allowedDropActions, allowedDropActions);
            }
        }

        public static void BeginDrag(
          DragSourceHandler sourceHandler,
          ICookedInputSite source,
          IRawInputSite target,
          int formX,
          int formY,
          InputModifiers modifiers)
        {
            DragDropHelper._sourceHandler = sourceHandler;
            DragDropHelper._draggingInternally = true;
            UISession.Default.Form.IsDragInProgress = true;
            UISession.Default.InputManager.SimulateDragEnter(source, target, DragDropHelper._sourceHandler.Value, formX, formY, modifiers);
        }

        public static void Requery(InputModifiers modifiers)
        {
            UISession.Default.InputManager.SimulateDragOver(modifiers);
            DragDropHelper._sourceHandler.UpdateCurrentAction();
        }

        public static void Requery(
          IRawInputSite target,
          int formX,
          int formY,
          InputModifiers modifiers)
        {
            UISession.Default.InputManager.SimulateDragOver(target, formX, formY, modifiers);
        }

        public static void EndDrag(IRawInputSite target, InputModifiers modifiers, DropAction action)
        {
            DragOperation formOperation = DragOperation.Drop;
            DragDropHelper._dropAction = action;
            DragDropHelper._draggingInternally = false;
            if (action == DropAction.None)
            {
                target = null;
                formOperation = DragOperation.Leave;
            }
            UISession.Default.InputManager.SimulateDragEnd(target, modifiers, formOperation);
            UISession.Default.Form.IsDragInProgress = false;
        }

        public static void OnDragComplete()
        {
            DragDropHelper._sourceHandler.OnEndDrag(DragDropHelper._dropAction);
            DragDropHelper._sourceHandler = null;
            DragDropHelper._dropAction = DropAction.None;
        }

        public static object GetValue() => UISession.Default.InputManager.GetDragDropValue();
    }
}
