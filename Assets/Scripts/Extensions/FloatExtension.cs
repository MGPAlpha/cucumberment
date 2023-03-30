using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtension
{

    public static float SignWithZero(float val) {
        if (val == 0) return 0;
        return Mathf.Sign(val);
    }

    public static float ChooseClosestTo(float target, float opt1, float opt2) {
        float diff1 = Mathf.Abs(opt1-target);
        float diff2 = Mathf.Abs(opt2-target);
        if (diff1 < diff2) {
            return opt1;
        } else {
            return opt2;
        }
    }

}
