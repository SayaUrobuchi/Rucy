using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RawImage))]
public class UVMaid : MonoBehaviour
{
    public enum FitType
    {
        RefToW, 
        RefToH, 
        Fill, 
        NotFill, 
    }
    
    public FitType RefType = FitType.RefToH;

    private RawImage target;

    public Rect UV
    {
        get
        {
            Init();
            return target.uvRect;
        }

        set
        {
            Init();
            target.uvRect = value;
        }
    }

    public Texture Texture
    {
        get
        {
            Init();
            return target.texture;
        }

        set
        {
            Init();
            target.texture = value;
        }
    }

	// Use this for initialization
	void Start ()
    {
        Init();
        if (!Application.isPlaying)
        {
            AdjustUV();
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (!Application.isPlaying)
        {
            AdjustUV();
        }
    }
#endif

    private void Init()
    {
        if (target == null)
        {
            target = GetComponent<RawImage>();
        }
    }

    [ContextMenu("AdjustUV")]
    public void AdjustUV()
    {
        Init();

        Texture tex = target.texture;
        if (tex != null)
        {
            Rect size = target.rectTransform.rect;
            Rect uv = target.uvRect;

            bool refX = (RefType == FitType.RefToW);
            switch (RefType)
            {
            case FitType.Fill:
                if (tex.width / (float)tex.height < size.x / size.y)
                {
                    refX = true;
                    uv.width = 1;
                }
                else
                {
                    uv.height = 1;
                }
                break;
            case FitType.NotFill:
                if (tex.width / (float)tex.height > size.x / size.y)
                {
                    refX = true;
                    uv.width = 1;
                }
                else
                {
                    uv.height = 1;
                }
                break;
            }

            if (refX)
            {
                float y = size.width * tex.height / tex.width;
                float h = size.height / y;
                uv.height = h * uv.width;
            }
            else
            {
                float x = size.height * tex.width / tex.height;
                float w = size.width / x;
                uv.width = w * uv.height;
            }
            target.uvRect = uv;
        }
    }
}
