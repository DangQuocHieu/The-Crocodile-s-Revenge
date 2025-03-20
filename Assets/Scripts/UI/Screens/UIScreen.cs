using DG.Tweening;
using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{
    public abstract IEnumerator Show(); 
    public abstract IEnumerator Hide();

}

