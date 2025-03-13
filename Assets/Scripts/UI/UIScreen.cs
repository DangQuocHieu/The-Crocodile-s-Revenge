using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{
    protected virtual void Awake()
    {
    }
    public abstract Tweener Show();
    public abstract Tweener Hide();

}

