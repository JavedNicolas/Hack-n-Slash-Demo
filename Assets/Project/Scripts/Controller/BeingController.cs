using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeingController 
{
    #region singleton
    public static BeingController instance = new BeingController();

    private BeingController() { }
    #endregion
}
