using UnityEngine;
using System.Collections.Generic;

public class ControlsDisplay : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Tecla para mostrar/ocultar los controles")]
    public KeyCode toggleKey = KeyCode.F1;

    [Tooltip("Color de fondo del panel")]
    public Color backgroundColor = new Color(0, 0, 0, 1f); // Fondo negro opaco

    [Tooltip("Color del texto")]
    public Color textColor = Color.white;

    [Header("Textos")]
    [Tooltip("Título de la pantalla de controles")]
    public string titleText = "CONTROLES DEL JUEGO";

    [Tooltip("Mensaje de cómo volver al juego (dejar vacío para no mostrar)")]
    public string returnMessage = "Presiona cualquier tecla para volver";

    [Header("Apariencia")]
    [Tooltip("Tamaño base del título (escalará con resolución)")]
    public int titleFontSize = 48;

    [Tooltip("Tamaño base del texto de los controles (escalará con resolución)")]
    public int controlsFontSize = 32;

    [Tooltip("Tamaño base del texto del mensaje (escalará con resolución)")]
    public int messageTextSize = 32;

    [Tooltip("Tamaño base de los iconos de teclas (escalará con resolución)")]
    public Vector2 keyIconSize = new Vector2(64, 64);

    [Tooltip("Margen del panel como porcentaje de la pantalla (0-1)")]
    [Range(0f, 0.1f)]
    public float panelMarginPercent = 0.02f;

    [Tooltip("Controles por fila (2 para dos columnas)")]
    public int controlsPerRow = 2;

    [System.Serializable]
    public class ControlBinding
    {
        public string action;
        public Sprite keyIcon;
    }

    [Header("Controles")]
    [Tooltip("Lista de acciones y sus teclas asociadas")]
    public List<ControlBinding> controlBindings = new List<ControlBinding>();

    private bool showControls = false;
    private GUIStyle titleStyle;
    private GUIStyle textStyle;
    private GUIStyle messageStyle;
    private Rect windowRect;

    private void Awake()
    {
        InitializeControls();
        ConfigureStyles();
        UpdateWindowRect();
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(titleText))
        {
            titleText = "CONTROLES DEL JUEGO";
        }
        Debug.Log("ControlsDisplay iniciado. Presiona " + toggleKey + " para mostrar los controles.");
    }

    private void InitializeControls()
    {
        if (controlBindings.Count == 0)
        {
            controlBindings.Add(new ControlBinding { action = "Mover arriba", keyIcon = null });
            controlBindings.Add(new ControlBinding { action = "Mover abajo", keyIcon = null });
            controlBindings.Add(new ControlBinding { action = "Mover izquierda", keyIcon = null });
            controlBindings.Add(new ControlBinding { action = "Mover derecha", keyIcon = null });
            controlBindings.Add(new ControlBinding { action = "Menu de mundos", keyIcon = null });
            controlBindings.Add(new ControlBinding { action = "Cerrar Juego", keyIcon = null });
        }
    }

    private void ConfigureStyles()
    {
        float scaleFactor = Screen.height / 1080f; // Escala basada en una resolución de referencia (1080p)

        titleStyle = new GUIStyle();
        titleStyle.fontSize = Mathf.RoundToInt(titleFontSize * scaleFactor);
        titleStyle.normal.textColor = textColor;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontStyle = FontStyle.Bold;

        textStyle = new GUIStyle();
        textStyle.fontSize = Mathf.RoundToInt(controlsFontSize * scaleFactor);
        textStyle.normal.textColor = textColor;
        textStyle.alignment = TextAnchor.MiddleLeft;
        textStyle.padding = new RectOffset(10, 10, 5, 5);

        messageStyle = new GUIStyle();
        messageStyle.fontSize = Mathf.RoundToInt(messageTextSize * scaleFactor);
        messageStyle.normal.textColor = textColor;
        messageStyle.alignment = TextAnchor.MiddleCenter;
        messageStyle.fontStyle = FontStyle.Bold;
    }

    private void UpdateWindowRect()
    {
        float margin = Screen.width * panelMarginPercent;
        windowRect = new Rect(
            margin,
            margin,
            Screen.width - (margin * 2),
            Screen.height - (margin * 2)
        );
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            showControls = !showControls;
            Debug.Log("Controles: " + (showControls ? "Mostrando" : "Ocultando"));
        }

        if (showControls && Input.anyKeyDown && !Input.GetKeyDown(toggleKey))
        {
            showControls = false;
            Debug.Log("Ocultando controles por presionar otra tecla");
        }
    }

    private void OnGUI()
    {
        if (windowRect.width != Screen.width - (Screen.width * panelMarginPercent * 2) ||
            windowRect.height != Screen.height - (Screen.height * panelMarginPercent * 2))
        {
            UpdateWindowRect();
        }

        ConfigureStyles();

        if (showControls)
        {
            GUI.color = backgroundColor;
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
            GUI.color = Color.white;

            float titleHeightPercent = 0.1f; // Reducido para bajar el contenido
            float controlsHeightPercent = 0.7f;
            float returnMessageHeightPercent = 0.1f; // Reducido para bajar el contenido
            float topPaddingPercent = 0.1f; // Espacio adicional en la parte superior para centrar

            float topPadding = Screen.height * topPaddingPercent;
            float titleHeight = Screen.height * titleHeightPercent;
            float controlsHeight = Screen.height * controlsHeightPercent;
            float returnMessageHeight = Screen.height * returnMessageHeightPercent;

            GUILayout.BeginArea(new Rect(0, topPadding, Screen.width, titleHeight));
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(titleText, titleStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(0, topPadding + titleHeight, Screen.width, controlsHeight));
            for (int i = 0; i < controlBindings.Count; i += controlsPerRow)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int j = 0; j < controlsPerRow && i + j < controlBindings.Count; j++)
                {
                    var control = controlBindings[i + j];
                    GUILayout.BeginVertical(GUILayout.Width(Screen.width / controlsPerRow * 0.9f));
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(control.action, textStyle);
                    GUILayout.Label(" : ", textStyle, GUILayout.Width(20));
                    if (control.keyIcon != null)
                    {
                        float iconScale = Screen.height / 1080f;
                        Rect iconRect = GUILayoutUtility.GetRect(keyIconSize.x * iconScale, keyIconSize.y * iconScale);
                        GUI.DrawTexture(iconRect, control.keyIcon.texture, ScaleMode.ScaleToFit);
                    }
                    else
                    {
                        GUILayoutUtility.GetRect(keyIconSize.x * (Screen.height / 1080f), keyIconSize.y * (Screen.height / 1080f));
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.Space(20);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Space(40);
            }
            GUILayout.EndArea();

            if (!string.IsNullOrEmpty(returnMessage))
            {
                Rect messageRect = new Rect(0, Screen.height - returnMessageHeight - topPadding, Screen.width, returnMessageHeight);
                GUI.Box(messageRect, "", new GUIStyle());
                GUILayout.BeginArea(messageRect);
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(returnMessage, messageStyle);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.EndArea();
            }
        }
    }
}