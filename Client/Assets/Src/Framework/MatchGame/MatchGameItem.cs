using XLua;
using UnityEngine;

namespace Framework
{
    public class MatchGameItem : MonoBehaviour
    {
        private Camera MatchCamera;
        private Rigidbody Rigidbody;
        private Vector3 SceenPoint;
        private Vector3 OffSet;

        private int TIME_INTERVAL = 6;
        private int HoldTimes = 0;

        private LuaFunction m_onMouseDown;
        private LuaFunction m_onMouseDrag;
        private LuaFunction m_onMouseUp;

        private void Start()
        {
            if (Lua.instance.m_matchLuaTable == null)
                Lua.instance.m_matchLuaTable = Lua.instance.GetScript("MatchGame.MatchItem");

            Rigidbody = GetComponent<Rigidbody>();
            m_onMouseDown = Lua.instance.m_matchLuaTable.GetInPath<LuaFunction>("onMouseDown");
            m_onMouseDrag = Lua.instance.m_matchLuaTable.GetInPath<LuaFunction>("onMouseDrag");
            m_onMouseUp = Lua.instance.m_matchLuaTable.GetInPath<LuaFunction>("onMouseUp");
            var parmas = Lua.instance.m_matchLuaTable.GetInPath<LuaFunction>("onInit").Call(this, gameObject);
            MatchCamera = parmas[0] as Camera;
        }
         
        private void OnMouseDown()
        {
            if (LuaHelper.CheckGuiRaycastObjects()) return;

            m_onMouseDown.Call();
            Rigidbody.useGravity = false;
            SceenPoint = MatchCamera.WorldToScreenPoint(gameObject.transform.position);
            OffSet = gameObject.transform.position - MatchCamera.ScreenToWorldPoint(
                new Vector3(
                    Input.mousePosition.x, 
                    Input.mousePosition.y, 
                    SceenPoint.z)
                );
        }

        private void OnMouseDrag()
        {
            if (LuaHelper.CheckGuiRaycastObjects()) return;

            HoldTimes++;
            if (HoldTimes >= TIME_INTERVAL)
            {
                m_onMouseDrag.Call(this, gameObject);
                var cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, SceenPoint.z);
                var cursorPosition = MatchCamera.ScreenToWorldPoint(cursorPoint) + OffSet;
                Rigidbody.position = cursorPosition;

                //var width = Screen.width / 2;
                var formatx = Rigidbody.position.x;
                //Debug.Log(width + "平行方向: " + formatx);
                if (Rigidbody.position.x < -104)
                    formatx = -104;
                else if (Rigidbody.position.x > -97.2)
                    formatx = -97.2f;

                var formatz = Rigidbody.position.z;
                if (formatz < -5.26)
                    formatz = -5.26f;
                else if (formatz > 4.05)
                    formatz = 4.05f;

                Rigidbody.MovePosition(new Vector3(formatx, 0.2f, formatz));
                Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }

        private void OnMouseUp()
        {
            if (LuaHelper.CheckGuiRaycastObjects()) return;

            Rigidbody.constraints = RigidbodyConstraints.None;
            if (HoldTimes < TIME_INTERVAL)
                m_onMouseUp.Call(this, gameObject);
            
            HoldTimes = 0;
            Rigidbody.useGravity = true;
        }
    }
}