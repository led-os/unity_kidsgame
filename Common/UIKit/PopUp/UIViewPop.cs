using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class UIViewPop : UIView
{


    public UnityEvent onOpen;
    public UnityEvent onClose;

    private Animator animator;

    /// <summary>
    /// Closes the popup.
    /// </summary>
    public void Close()
    {
        animator = this.gameObject.GetComponent<Animator>();
        // onClose.Invoke();
        // if (parentScene != null)
        // {
        //     parentScene.ClosePopup();
        // }
        PopUpManager.main.ClosePopup();
        if (animator != null)
        {
            animator.Play("Close");
            StartCoroutine(DestroyPopup());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Utility coroutine to automatically destroy the popup after its closing animation has finished.
    /// </summary>
    /// <returns>The coroutine.</returns>
    protected virtual IEnumerator DestroyPopup()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }


}
