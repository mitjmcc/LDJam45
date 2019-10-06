namespace HomeTakeover.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using Util;

    public static class Navigator
    {
        public static void Navigate(string direction, GameObject defaultGameObject)
        {
            GameObject next = EventSystem.current.currentSelectedGameObject;
            if (next == null)
            {
                if (defaultGameObject != null) EventSystem.current.SetSelectedGameObject(defaultGameObject);
                return;
            }

            bool nextIsValid = false;
            while (!nextIsValid)
            {
                // Don't switch on strings in non game jam settings boyo
                switch (direction)
                {
                    case "UI_Up":
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp() != null)
                            next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp().gameObject;
                        else next = null;
                        break;
                    case "UI_Down":
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown() != null)
                            next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown().gameObject;
                        else next = null;
                        break;
                    case "UI_Left":
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnLeft() != null)
                            next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnLeft().gameObject;
                        else next = null;
                        break;
                    case "UI_Right":
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight() != null)
                            next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight().gameObject;
                        else next = null;
                        break;
                }
                if (next != null && next.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(next);
                    nextIsValid = next.GetComponent<Selectable>().interactable;
                }
                else nextIsValid = true;
            }
        }

        public static void CallSubmit()
        {
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, pointer, ExecuteEvents.submitHandler);
        }
    }
}