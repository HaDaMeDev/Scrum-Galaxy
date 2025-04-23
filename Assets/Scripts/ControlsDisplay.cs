using UnityEngine;
using System.Collections.Generic;

public class ControlsDisplay : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Tecla para mostrar/ocultar los controles")]
    public KeyCode toggleKey = KeyCode.F1;
    
    [Tooltip("Color de fondo del panel")]
    public Color backgroundColor = new Color(0, 0, 0, 0.8f);
    
    [Tooltip("Color del texto")]
    public Color textColor = Color.white;
    
    [Header("Textos")]
    [Tooltip("Título de la pantalla de controles")]
    public string titleText = "CONTROLES DEL JUEGO";
    
    [Tooltip("Mensaje de cómo volver al juego (dejar vacío para no mostrar)")]
    public string returnMessage = "Presiona cualquier tecla para volver";
    
    [Header("Apariencia")]
    [Tooltip("Tamaño del título")]
    public int titleFontSize = 36;
    
    [Tooltip("Tamaño del texto de los controles")]
    public int controlsFontSize = 24;
    
    [Tooltip("Tamaño del texto del mensaje")]
    public int messageTextSize = 24;
    
    [Tooltip("Tamaño de los iconos de teclas")]
    public Vector2 keyIconSize = new Vector2(64, 64);
    
    [Tooltip("Margen del panel respecto a los bordes de la pantalla")]
    public int panelMargin = 20;
    
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
        // Inicializar componentes
        InitializeControls();
        ConfigureStyles();
        UpdateWindowRect();
    }
    
    private void Start()
    {
        // Inicializar con valor por defecto si está vacío
        if (string.IsNullOrEmpty(titleText))
        {
            titleText = "CONTROLES DEL JUEGO";
        }
        
        // Debug para verificar que el script se está ejecutando
        Debug.Log("ControlsDisplay iniciado. Presiona " + toggleKey + " para mostrar los controles.");
    }
    
    private void InitializeControls()
    {
        // Inicializar con los controles de movimiento con flechas si la lista está vacía
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
        // Configurar el estilo del título
        titleStyle = new GUIStyle();
        titleStyle.fontSize = titleFontSize;
        titleStyle.normal.textColor = textColor;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontStyle = FontStyle.Bold;
        
        // Configurar el estilo del texto
        textStyle = new GUIStyle();
        textStyle.fontSize = controlsFontSize;
        textStyle.normal.textColor = textColor;
        textStyle.alignment = TextAnchor.MiddleLeft;
        textStyle.padding = new RectOffset(10, 10, 5, 5);
        
        // Configurar el estilo del mensaje
        messageStyle = new GUIStyle();
        messageStyle.fontSize = messageTextSize;
        messageStyle.normal.textColor = textColor;
        messageStyle.alignment = TextAnchor.MiddleCenter;
        messageStyle.fontStyle = FontStyle.Bold; // Más visible
    }
    
    private void UpdateWindowRect()
    {
        // Configurar el rectángulo de la ventana para que ocupe toda la pantalla
        windowRect = new Rect(
            panelMargin,
            panelMargin,
            Screen.width - (panelMargin * 2),
            Screen.height - (panelMargin * 2)
        );
    }
    
    private void Update()
    {
        // Comprobar si se ha pulsado la tecla para mostrar/ocultar los controles
        if (Input.GetKeyDown(toggleKey))
        {
            showControls = !showControls;
            Debug.Log("Controles: " + (showControls ? "Mostrando" : "Ocultando"));
        }
        
        // Si los controles están visibles y se presiona CUALQUIER tecla (excepto la que ya verificamos)
        if (showControls && Input.anyKeyDown && !Input.GetKeyDown(toggleKey))
        {
            showControls = false;
            Debug.Log("Ocultando controles por presionar otra tecla");
        }
    }
    
    private void OnGUI()
    {
        // Actualizar el tamaño de la ventana en caso de que la pantalla cambie de tamaño
        if (windowRect.width != Screen.width - (panelMargin * 2) || 
            windowRect.height != Screen.height - (panelMargin * 2))
        {
            UpdateWindowRect();
        }
        
        // Actualizar estilos en caso de que se hayan modificado los parámetros
        ConfigureStyles();
        
        if (showControls)
        {
            // Dibujar un fondo semi-transparente que cubra toda la pantalla
            GUI.color = backgroundColor;
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
            GUI.color = Color.white;
            
            // Calcular la altura disponible para los controles
            float totalHeight = Screen.height;
            float titleHeight = 150; // Espacio para el título
            float returnMessageHeight = 80; // Espacio para el mensaje de retorno
            float controlsHeight = totalHeight - titleHeight - returnMessageHeight;
            
            // Dibujar el título
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, titleHeight));
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(titleText, titleStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndArea();
            
            // Dibujar los controles
            GUILayout.BeginArea(new Rect(0, titleHeight, Screen.width, controlsHeight));
            
            // Organizar controles en filas
            for (int i = 0; i < controlBindings.Count; i += controlsPerRow)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                
                // Mostrar controles en esta fila (máximo 'controlsPerRow' controles)
                for (int j = 0; j < controlsPerRow && i + j < controlBindings.Count; j++)
                {
                    var control = controlBindings[i + j];
                    
                    // Un grupo para cada control
                    GUILayout.BeginVertical(GUILayout.Width(300));
                    
                    // Mostrar la acción
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(control.action, textStyle);
                    GUILayout.Label(" : ", textStyle, GUILayout.Width(20));
                    
                    // Mostrar el icono si existe
                    if (control.keyIcon != null)
                    {
                        Rect iconRect = GUILayoutUtility.GetRect(keyIconSize.x, keyIconSize.y);
                        GUI.DrawTexture(iconRect, control.keyIcon.texture, ScaleMode.ScaleToFit);
                    }
                    else
                    {
                        // Espacio reservado para el icono
                        GUILayoutUtility.GetRect(keyIconSize.x, keyIconSize.y);
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    
                    GUILayout.EndVertical();
                    
                    // Espacio entre controles en la misma fila
                    GUILayout.Space(20);
                }
                
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                
                // Espacio entre filas
                GUILayout.Space(40);
            }
            
            GUILayout.EndArea();
            
            // Dibujar el mensaje de retorno en un área específica en la parte inferior
            if (!string.IsNullOrEmpty(returnMessage))
            {
                Rect messageRect = new Rect(0, Screen.height - returnMessageHeight, Screen.width, returnMessageHeight);
                GUI.Box(messageRect, "", new GUIStyle()); // Contenedor invisible para el mensaje
                
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