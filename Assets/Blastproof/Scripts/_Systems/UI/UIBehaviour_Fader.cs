using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIBehaviour_Fader : UIBehaviour_Component
{
    private Image img;
    private Image Img => img ?? (img = GetComponentInChildren<Image>(true));
    
    public override void Activated()
    {
        //Debug.Log("Activated");
        Img.DOFade(.5f, .33f);
        Img.raycastTarget = true;
    }

    public override void Deactivated()
    {
        //Debug.Log("Deactivated");
        Img.DOFade(0f, .33f);
        Img.raycastTarget = false;
    }
}
