using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace YIUIFramework
{
    /// <summary>
    /// 点击穿透
    /// 只支持穿透一层
    /// </summary>
    [AddComponentMenu("YIUIFramework/Widget/点击穿透 【YIUIClickEventPenetration】")]
    public class YIUIClickEventPenetration : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.pointerClickHandler);
        }

        private void PassEvent<T>(PointerEventData pointerEventData, ExecuteEvents.EventFunction<T> eventFunction) where T : IEventSystemHandler
        {
            var raycastResults = new List<RaycastResult>();

            //获取射线检测结果
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            for (int i = 0; i < raycastResults.Count; i++)
            {
                GameObject resultGameObject = raycastResults[i].gameObject;

                //如果不是自身则将事件传递下去
                if (resultGameObject != null && resultGameObject != this.gameObject)
                {
                    //目前只是传递一层如需多层把break去掉即可
                    ExecuteEvents.Execute(resultGameObject, pointerEventData, eventFunction);
                    break;
                }
            }
        }
    }

    /*

    1.下层Button没响应问题

    如果只传递一层的情况 Button的Text文本 或者Button下面有遮挡物 并且Raycast Target是true的情况
    它会传递给Text或者 其他遮挡物 而Button 不会收到点击消息 所以如果想要下层百分百生效 最好的情况是
    把Button 下面的遮挡物 Raycast Target设置为false 因为如果一直往下传递的话
    当前点击的位置无论叠加多少层Button 他都会响应 所以一直往下传递是不可控的情况

    2.下层Toggle没响应问题

    再有一种情况是Toggle 我在UI上的Toggle 自身没有Image 组件 Toggle的Target Graphic
    设置的是Background 然后事件传递到Backgeround 之后 Toggle并没有响应 所以如果想让Toggle也响应该事件
    需要将Toggle的Target Graphic设置为自身 给Toggle添加一个Image 并且Raycast Target 设置为true 并且没有遮挡物即可

    总结注意下一层是什么 有可能是你不知道的东西阻挡了 所以没有达到你的效果
     */
}