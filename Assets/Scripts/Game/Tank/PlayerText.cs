using UnityEngine;

public class PlayerText : MonoBehaviour
{
    [SerializeField] Vector2 mWorldOffset = Vector2.zero;
    [SerializeField] Transform mText = null;
    
    void LateUpdate()
    {
        //Position des Canvas
        Vector2 position = new Vector2(transform.position.x + mWorldOffset.x,
                                       transform.position.y + mWorldOffset.y);

        mText.position = position;
    }
}
