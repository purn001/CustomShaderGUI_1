using System;
using UnityEngine;
using UnityEditor;
 
public class FX_ParticleDissolveUseCustomDataGUI: ShaderGUI
{
    private static class Styles
    {
        public static GUIContent MainTexText = EditorGUIUtility.TrTextContent("Main Texture"); 
        public static GUIContent AlphaTexText = EditorGUIUtility.TrTextContent("Alpha Texture");  
        public static GUIContent DissolveTexText = EditorGUIUtility.TrTextContent("Dissolve Texture");  
        public static GUIContent DeformTexText = EditorGUIUtility.TrTextContent("Deform Texture");       
        // public static string MainTexText = "메인 텍스쳐";
    }

    // enum AddGradation {
	// 	NONE, 
    //     X, 
    //     Y
	// }

    enum BlendMode {
		Zero, One, DstColor, SrcColor, OneMinusDstColor, SrcAlpha, OneMinusSrcColor, DstAlpha, OneMinusDstAlpha, SrcAlphaSaturate, OneMinusSrcAlpha 
	}

    private Material m_Material;
    private MaterialEditor m_MaterialEditor;
    // private bool isGUIInitialized = false;
    // private GUIStyle style = null;

    #region MaterialProperty
    MaterialProperty MainTex = null;
    MaterialProperty mColor = null;
    MaterialProperty MainAngle = null;
    MaterialProperty MainSpeed = null;
    MaterialProperty MainSpeed_X = null;
    MaterialProperty MainSpeed_Y = null;
    MaterialProperty Main_Flow_CustomData = null;
    MaterialProperty Main_Polar_Coordiante = null;

    MaterialProperty AlphaToggleMenu = null;
    MaterialProperty AlphaTex = null;
    MaterialProperty AlphaAngle = null;
    MaterialProperty AlphaSpeed = null;
    MaterialProperty AlphaSpeed_X = null;
    MaterialProperty AlphaSpeed_Y = null;
    MaterialProperty Alpha_Flow_CustomData = null;
    MaterialProperty Alpha_Polar_Coordiante = null;

    MaterialProperty DissolveToggleMenu = null;
    MaterialProperty DissolveTex = null;
    MaterialProperty DissolveAngle = null;
    MaterialProperty DissolveSpeed = null;
    MaterialProperty DissolveSpeed_X = null;
    MaterialProperty DissolveSpeed_Y = null;
    MaterialProperty Dissolve_Flow_CustomData = null;
    MaterialProperty Dissolve_Polar_Coordiante = null;
    // MaterialProperty OutlineToggleMenu = null;
    MaterialProperty DissolveOutRange = null;
    MaterialProperty SmoothToggleMenu = null;
    MaterialProperty SmoothRange = null;
    // MaterialProperty GradationToggleMenu = null;

    MaterialProperty NoiseToggleMenu = null;
    MaterialProperty NoiseTex = null;
    MaterialProperty NoiseAngle = null;
    MaterialProperty NoiseSpeed = null;
    MaterialProperty NoiseSpeed_X = null;
    MaterialProperty NoiseSpeed_Y = null;
    MaterialProperty Noise_Polar_Coordiante = null;
    MaterialProperty MainFlowIntensity = null;
    MaterialProperty AlphaFlowIntensity = null;
    MaterialProperty DissolveFlowIntensity = null;

    // MaterialProperty SrcBlend = null;
    #endregion
    
    static GUIContent staticLabel = new GUIContent();
   
    public void FindProperties(MaterialProperty[] props)  //받아올 프로퍼티 받아오기
    {
        MainTex = FindProperty("_MainTex", props);
        mColor = FindProperty("_Color", props);
        MainAngle = FindProperty("_mAngle", props);
        MainSpeed = FindProperty("_mSpeed", props);
        MainSpeed_X = FindProperty("_mX", props);
        MainSpeed_Y = FindProperty("_mY", props);
        Main_Flow_CustomData = FindProperty("_Main_Flow_CustomData", props);
        Main_Polar_Coordiante = FindProperty("_Main_Polar_Coordiante", props);

        AlphaToggleMenu = FindProperty("_Use_Alpha", props);
        AlphaTex = FindProperty("_AlphaTex", props);
        AlphaAngle = FindProperty("_aAngle", props);
        AlphaSpeed = FindProperty("_aSpeed", props);
        AlphaSpeed_X = FindProperty("_aX", props);
        AlphaSpeed_Y = FindProperty("_aY", props);
        Alpha_Flow_CustomData = FindProperty("_Alpha_Flow_CustomData", props);
        Alpha_Polar_Coordiante = FindProperty("_Alpha_Polar_Coordiante", props);

        DissolveToggleMenu = FindProperty("_Use_Dissolve", props);
        DissolveTex = FindProperty("_DisolveTex", props);
        DissolveAngle = FindProperty("_dAngle", props);
        DissolveSpeed = FindProperty("_dSpeed", props);
        DissolveSpeed_X = FindProperty("_dX", props);
        DissolveSpeed_Y = FindProperty("_dY", props);
        Dissolve_Flow_CustomData = FindProperty("_Dissolve_Flow_CustomData", props);
        Dissolve_Polar_Coordiante = FindProperty("_Dissolve_Polar_Coordiante", props);
        //OutlineToggleMenu = FindProperty("_Use_Outline", props);
        DissolveOutRange = FindProperty("_disOutRange", props);
        SmoothToggleMenu = FindProperty("_Use_Smooth", props);
        SmoothRange = FindProperty("_smoothRange", props);
        //GradationToggleMenu = FindProperty("_Gradation_Inverse", props);

        NoiseToggleMenu = FindProperty("_Use_Noise", props);
        NoiseTex = FindProperty("_NoiseTex", props);
        NoiseAngle = FindProperty("_nAngle", props);
        NoiseSpeed = FindProperty("_nSpeed", props);
        NoiseSpeed_X = FindProperty("_nX", props);
        NoiseSpeed_Y = FindProperty("_nY", props);
        Noise_Polar_Coordiante = FindProperty("_Noise_Polar_Coordiante", props);
        MainFlowIntensity = FindProperty("_mFlowIntensity", props);
        AlphaFlowIntensity = FindProperty("_aFlowIntensity", props);
        DissolveFlowIntensity = FindProperty("_dFlowIntensity", props);

        //SrcBlend = FindProperty("_SrcBlend", props);
    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        FindProperties(props);
        m_MaterialEditor = materialEditor;
        m_Material = materialEditor.target as Material;

        ShaderPropertiesGUI(m_Material);
    }

    public void ShaderPropertiesGUI(Material material)
    {
        // EditorGUIUtility.labelWidth = 0f;
        EditorGUI.BeginChangeCheck();
        {   
            EditorGUILayout.Space(10);
            // InitGUIStyles();
            DoMainTex(material);
            GuiLine();
            DoAlpha(material);
            GuiLine();
            DoDissolve(material);
            GuiLine();
            DoDeform(material);
            GuiLine();
            EditorGUILayout.Space(10);
            SrcBlendModePopup(material, (BlendMode)material.GetInt("_SrcBlend"));
            DstBlendModePopup(material, (BlendMode)material.GetInt("_DstBlend"));
            EditorGUILayout.Space(20);
            m_MaterialEditor.RenderQueueField(); 
        }
        EditorGUI.EndChangeCheck();
             
    }

    void DoMainTex(Material material)
    {
        m_MaterialEditor.TexturePropertySingleLine(Styles.MainTexText, MainTex, mColor);
        EditorGUI.indentLevel += 2;
        m_MaterialEditor.TextureScaleOffsetProperty(MainTex);

        m_MaterialEditor.ShaderProperty(Main_Flow_CustomData, MakeLabel(Main_Flow_CustomData));
        SetKeyword("_MAIN_FLOW_CUSTOMDATA", material.GetInt("_Main_Flow_CustomData") == 1);

        m_MaterialEditor.ShaderProperty(Main_Polar_Coordiante, MakeLabel(Main_Polar_Coordiante));
        SetKeyword("_MAIN_POLAR_COORDINATE", material.GetInt("_Main_Polar_Coordiante") == 1);

        m_MaterialEditor.ShaderProperty(MainAngle, MakeLabel(MainAngle));
        m_MaterialEditor.ShaderProperty(MainSpeed, MakeLabel(MainSpeed));
        m_MaterialEditor.ShaderProperty(MainSpeed_X, MakeLabel(MainSpeed_X));
        m_MaterialEditor.ShaderProperty(MainSpeed_Y, MakeLabel(MainSpeed_Y));
        EditorGUI.indentLevel -= 2;
    }

    void DoAlpha(Material material)
    {
        // EditorGUILayout.BeginVertical(GUI.skin.GetStyle("HelpBox"));
        // {
        m_MaterialEditor.ShaderProperty(AlphaToggleMenu, MakeLabel(AlphaToggleMenu));
        // Debug.Log(Convert.ToInt32(on));
        if (material.GetInt("_Use_Alpha") == 1)
        {   
            material.EnableKeyword("_USE_ALPHA");
            m_MaterialEditor.TexturePropertySingleLine(Styles.AlphaTexText, AlphaTex);
            EditorGUI.indentLevel += 2;
            m_MaterialEditor.TextureScaleOffsetProperty(AlphaTex);

            m_MaterialEditor.ShaderProperty(Alpha_Flow_CustomData, MakeLabel(Alpha_Flow_CustomData));
            SetKeyword("_ALPHA_FLOW_CUSTOMDATA", material.GetInt("_Alpha_Flow_CustomData") == 1);

            m_MaterialEditor.ShaderProperty(Alpha_Polar_Coordiante, MakeLabel(Alpha_Polar_Coordiante));
            SetKeyword("_ALPHA_POLAR_COORDINATE", material.GetInt("_Alpha_Polar_Coordiante") == 1); 

            m_MaterialEditor.ShaderProperty(AlphaAngle, MakeLabel(AlphaAngle));
            m_MaterialEditor.ShaderProperty(AlphaSpeed, MakeLabel(AlphaSpeed));
            m_MaterialEditor.ShaderProperty(AlphaSpeed_X, MakeLabel(AlphaSpeed_X));
            m_MaterialEditor.ShaderProperty(AlphaSpeed_Y, MakeLabel(AlphaSpeed_Y));
            EditorGUI.indentLevel -= 2;
        }
        else{
            material.DisableKeyword("_USE_ALPHA");
        }
        // }
        // EditorGUILayout.EndVertical();
    }

    void DoDissolve(Material material)
    {
        m_MaterialEditor.ShaderProperty(DissolveToggleMenu, MakeLabel(DissolveToggleMenu));
        if (material.GetInt("_Use_Dissolve") == 1)
        {   
            //GUILayout.Label(Styles.AlphaMenuONText, EditorStyles.boldLabel);
            material.EnableKeyword("_USE_DISSOLVE");
            m_MaterialEditor.TexturePropertySingleLine(Styles.DissolveTexText, DissolveTex);
            EditorGUI.indentLevel += 2;
            m_MaterialEditor.TextureScaleOffsetProperty(DissolveTex);

            m_MaterialEditor.ShaderProperty(Dissolve_Flow_CustomData, MakeLabel(Dissolve_Flow_CustomData));
            SetKeyword("_DISSOLVE_FLOW_CUSTOMDATA", material.GetInt("_Dissolve_Flow_CustomData") == 1);

            m_MaterialEditor.ShaderProperty(Dissolve_Polar_Coordiante, MakeLabel(Dissolve_Polar_Coordiante));
            SetKeyword("_DISSOLVE_POLAR_COORDINATE", material.GetInt("_Dissolve_Polar_Coordiante") == 1); 

            // DOAddGradation(material, (AddGradation)material.GetInt("_AddGradation"));

            // m_MaterialEditor.ShaderProperty(OutlineToggleMenu, MakeLabel(OutlineToggleMenu));
            // if (material.GetInt("_Use_Outline") == 1)
            // {
            // material.EnableKeyword("_USE_OUTLINE");
            EditorGUI.indentLevel += 2;
            m_MaterialEditor.ShaderProperty(DissolveOutRange, MakeLabel(DissolveOutRange));
            EditorGUI.indentLevel -= 2;
            // }
            // else
            //     material.DisableKeyword("_USE_OUTLINE"); 

            m_MaterialEditor.ShaderProperty(SmoothToggleMenu, MakeLabel(SmoothToggleMenu));
            if (material.GetInt("_Use_Smooth") == 1)
            {
                material.EnableKeyword("_USE_SMOOTH");
                EditorGUI.indentLevel += 2;
                m_MaterialEditor.ShaderProperty(SmoothRange, MakeLabel(SmoothRange));
                EditorGUI.indentLevel -= 2;
            }
            else
                material.DisableKeyword("_USE_SMOOTH");        
         
            m_MaterialEditor.ShaderProperty(DissolveAngle, MakeLabel(DissolveAngle));
            m_MaterialEditor.ShaderProperty(DissolveSpeed, MakeLabel(DissolveSpeed));
            m_MaterialEditor.ShaderProperty(DissolveSpeed_X, MakeLabel(DissolveSpeed_X));
            m_MaterialEditor.ShaderProperty(DissolveSpeed_Y, MakeLabel(DissolveSpeed_Y));
            EditorGUI.indentLevel -= 2;
        }
        else{
            material.DisableKeyword("_USE_DISSOLVE");
        }
    }

    // void DOAddGradation(Material material, AddGradation addGradation)
    // {
    //     if (IsKeywordEnabled("_ADDGRADATION_NONE")) {
	// 	    addGradation = AddGradation.NONE;
	// 	}
	// 	else if (IsKeywordEnabled("_ADDGRADATION_X")) {
	// 	    addGradation = AddGradation.X;
	// 	}
	// 	else if (IsKeywordEnabled("_ADDGRADATION_Y")) {
	// 	    addGradation = AddGradation.Y;
	// 	}

    //     addGradation = (AddGradation)EditorGUILayout.EnumPopup(MakeLabel("Add X, Y Gradation?"), addGradation);
    //     switch(addGradation)
    //     {
    //         case AddGradation.NONE:	                 
    //             addGradation = AddGradation.NONE;         
    //             SetKeyword("_ADDGRADATION_NONE", addGradation == AddGradation.NONE);            
    //             break;
    //         case AddGradation.X:	   
    //             addGradation = AddGradation.X;
    //             SetKeyword("_ADDGRADATION_X", addGradation == AddGradation.X);                     
    //             break;
    //         case AddGradation.Y:	   
    //             addGradation = AddGradation.Y;
    //             SetKeyword("_ADDGRADATION_Y", addGradation == AddGradation.Y);                     
    //             break;
    //     }
        
    //     // RecordAction("Add X, Y Gradation?");       
    //     // SetKeyword("_ADDGRADATION_NONE", addGradation == AddGradation.NONE); 
    //     // SetKeyword("_ADDGRADATION_X", addGradation == AddGradation.X);
	// 	// SetKeyword("_ADDGRADATION_Y", addGradation == AddGradation.Y);

    //     if(addGradation != AddGradation.NONE){
    //         EditorGUI.indentLevel += 2;
    //         bool _Gradation_Inverse = EditorPrefs.GetBool(uniqueKey + "_Gradation_Inverse", false);         
    //         _Gradation_Inverse = EditorGUILayout.Toggle(MakeLabel(GradationToggleMenu), _Gradation_Inverse);
    //         EditorPrefs.SetBool(uniqueKey + "_Gradation_Inverse", _Gradation_Inverse); 
    //         EditorGUI.indentLevel -= 2;
    //         if (_Gradation_Inverse)
    //              material.EnableKeyword("_GRADATION_INVERSE");
    //         else
    //             material.DisableKeyword("_GRADATION_INVERSE");
    //     }
    // }

    void DoDeform(Material material)
    {
        m_MaterialEditor.ShaderProperty(NoiseToggleMenu, MakeLabel(NoiseToggleMenu));
        if (material.GetInt("_Use_Noise") == 1)
        {   
            material.EnableKeyword("_USE_NOISE");
            //GUILayout.Label(Styles.AlphaMenuONText, EditorStyles.boldLabel);
            m_MaterialEditor.TexturePropertySingleLine(Styles.DeformTexText, NoiseTex);
            EditorGUI.indentLevel += 2;
            m_MaterialEditor.TextureScaleOffsetProperty(NoiseTex);

            m_MaterialEditor.ShaderProperty(Noise_Polar_Coordiante, MakeLabel(Noise_Polar_Coordiante));
            SetKeyword("_NOISE_POLAR_COORDINATE", material.GetInt("_Noise_Polar_Coordiante") == 1); 

            m_MaterialEditor.ShaderProperty(NoiseAngle, MakeLabel(NoiseAngle));
            m_MaterialEditor.ShaderProperty(NoiseSpeed, MakeLabel(NoiseSpeed));
            m_MaterialEditor.ShaderProperty(NoiseSpeed_X, MakeLabel(NoiseSpeed_X));
            m_MaterialEditor.ShaderProperty(NoiseSpeed_Y, MakeLabel(NoiseSpeed_Y));

            m_MaterialEditor.ShaderProperty(MainFlowIntensity, MakeLabel(MainFlowIntensity));
            m_MaterialEditor.ShaderProperty(AlphaFlowIntensity, MakeLabel(AlphaFlowIntensity));
            m_MaterialEditor.ShaderProperty(DissolveFlowIntensity, MakeLabel(DissolveFlowIntensity));
            EditorGUI.indentLevel -= 2;
        }
        else{
            material.DisableKeyword("_USE_NOISE");
        }
    }

    void SrcBlendModePopup(Material material, BlendMode blendMode)
    {         
        blendMode = (BlendMode)EditorGUILayout.EnumPopup(MakeLabel("Blend Mode"), blendMode);
        switch(blendMode)
        {
            case BlendMode.Zero:	                 
                blendMode = BlendMode.Zero;         
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.Zero);               
                break;
            case BlendMode.One:	   
                blendMode = BlendMode.One;
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);                       
                break;
            case BlendMode.DstColor:	   
                blendMode = BlendMode.DstColor;
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.DstColor);                       
                break;  
            case BlendMode.SrcColor:	   
                blendMode = BlendMode.SrcColor;
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcColor);                       
                break;  
            case BlendMode.OneMinusDstColor:	   
                blendMode = BlendMode.OneMinusDstColor;
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstColor);                       
                break;  
            case BlendMode.SrcAlpha:	   
                blendMode = BlendMode.SrcAlpha;
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);                       
                break;  
            case BlendMode.OneMinusSrcColor:	   
                blendMode = BlendMode.OneMinusSrcColor;
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcColor);                       
                break;  
            case BlendMode.DstAlpha:	   
                blendMode = BlendMode.DstAlpha;
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.DstAlpha);                       
                break;  
            case BlendMode.OneMinusDstAlpha:	   
                blendMode = BlendMode.OneMinusDstAlpha;
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstAlpha);                       
                break;  
            case BlendMode.SrcAlphaSaturate:	   
                blendMode = BlendMode.SrcAlphaSaturate;
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlphaSaturate);                       
                break;     
            case BlendMode.OneMinusSrcAlpha:	   
                blendMode = BlendMode.OneMinusSrcAlpha;
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);                       
                break;   
		}
    }
    void DstBlendModePopup(Material material, BlendMode blendMode)
    {         
        blendMode = (BlendMode)EditorGUILayout.EnumPopup(MakeLabel("          "),blendMode);
        switch(blendMode)
        {
            case BlendMode.Zero:	                 
                blendMode = BlendMode.Zero;         
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);               
                break;
            case BlendMode.One:	   
                blendMode = BlendMode.One;
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);                       
                break;
            case BlendMode.DstColor:	   
                blendMode = BlendMode.DstColor;
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.DstColor);                       
                break;  
            case BlendMode.SrcColor:	   
                blendMode = BlendMode.SrcColor;
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.SrcColor);                       
                break;  
            case BlendMode.OneMinusDstColor:	   
                blendMode = BlendMode.OneMinusDstColor;
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstColor);                       
                break;  
            case BlendMode.SrcAlpha:	   
                blendMode = BlendMode.SrcAlpha;
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);                       
                break;  
            case BlendMode.OneMinusSrcColor:	   
                blendMode = BlendMode.OneMinusSrcColor;
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcColor);                       
                break;  
            case BlendMode.DstAlpha:	   
                blendMode = BlendMode.DstAlpha;
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.DstAlpha);                       
                break;  
            case BlendMode.OneMinusDstAlpha:	   
                blendMode = BlendMode.OneMinusDstAlpha;
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstAlpha);                       
                break;  
            case BlendMode.SrcAlphaSaturate:	   
                blendMode = BlendMode.SrcAlphaSaturate;
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlphaSaturate);                       
                break;     
            case BlendMode.OneMinusSrcAlpha:	   
                blendMode = BlendMode.OneMinusSrcAlpha;
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);                       
                break;   
		}
    }

     static GUIContent MakeLabel(MaterialProperty property, string tooltip = null)
    {
        staticLabel.text = property.displayName;
        staticLabel.tooltip = tooltip;
        return staticLabel;
    }

    static GUIContent MakeLabel (string text, string tooltip = null) {
		staticLabel.text = text;
		staticLabel.tooltip = tooltip;
		return staticLabel;
	}

    // bool IsKeywordEnabled (string keyword) {
	// 	return m_Material.IsKeywordEnabled(keyword);
	// }

    void SetKeyword (string keyword, bool state) {
		if (state) {
			m_Material.EnableKeyword(keyword);
		}
		else {
			m_Material.DisableKeyword(keyword);
		}
	}

    // void RecordAction (string label) {
	// 	m_MaterialEditor.RegisterPropertyChangeUndo(label);
	// }

    void GuiLine(int i_height = 1)
    {
       // EditorGUILayout.Space(10);
       Rect rect = EditorGUILayout.GetControlRect(false, i_height );
       rect.height = i_height;
       EditorGUI.DrawRect(rect, new Color (0.0f, 0.5f, 0.0f, 1));     
    }

    // void InitGUIStyles()
    // {
    //     if (isGUIInitialized) return;
        
    //     style = new GUIStyle(GUI.skin.button);
    //     style.normal.textColor = Color.gray;
    //     style.fontSize = 20;
    //     style.fontStyle = FontStyle.Bold;
    //     style.wordWrap = true;
    //     isGUIInitialized = true;
    // }
}
