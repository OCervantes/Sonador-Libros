using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using DocumentStore = System.Collections.Generic.Dictionary<string, object>;

public class VocabularyManager: MonoBehaviour,
    FirebaseManager.OnMissionDataRecievedCallback
{
    //Datos Miembro
    public AudioSource bounceSource, lockSource;
    public GameObject recibidor, banco, definition,
      prefabMovableAndSlot = null, prefabSlot = null, cameraObject = null;
    public bool butDoesItSave = false;
    public string missionName = "hangman_1";

    GameObject MovAndSlotObjRef, SlotObjRef;
    List<string> letrasPal = new List<string>();
    bool hasDestroyedRecibidor = false, hasDestroyedBanco = false;
    int savedIndex;
    string recoveredVocabWord;

    /* Lo tuve que volver estático porque no me permitía de otra manera.
     * Tendré que revisar que no interfiera más tarde.
     * Esta vez, upperRNGLimit es 3 posiciones superior a lowerRNGLimit, porque al parecer el método Random.
     * Range() es inclusivo en cuanto a su límite inferior, pero exclusivo para el superior.
     */
    static int lowerRNGLimit = 0, upperRNGLimit = lowerRNGLimit+3;

    /* Declaración e Inicialización de la Matriz
     * Palabra, Definición
     * 30 elementos--3 elementos por nivel; 10 niveles.
     */
    private string[,] matrix;

    /* Arreglo de enteros. Posición del entero proporcional a la palabra a aparecer en aquel mismo nivel.
     * Contiene 30 enteros, equivalente a las 30 palabras almacenadas en la matriz.
     * Entero almacenado corresponde al nivel de la palabra.
     */
    private int[] nivel;

    // Arreglos necesitan ser 'private'
    private bool[] matrixIndexAvailability;

    //Métodos
    public void Start()
    {
        FirebaseManager.GetMissionData(this, missionName);
    }

// Código reciclado que se encarga del ajuste adecuado de la Escala del Game View.
#if UNITY_EDITOR
  void Awake()
  {
      SetGameViewScale();
  }

  void SetGameViewScale()
  {
      System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
      System.Type type = assembly.GetType("UnityEditor.GameView");
      UnityEditor.EditorWindow v = UnityEditor.EditorWindow.GetWindow(type);

      var defScaleField = type.GetField("m_defaultScale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

      //whatever scale you want when you click on play
      float defaultScale = 0.37f;

      var areaField = type.GetField("m_ZoomArea", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
      var areaObj = areaField.GetValue(v);

      var scaleField = areaObj.GetType().GetField("m_Scale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
      scaleField.SetValue(areaObj, new Vector2(defaultScale, defaultScale));
  }
#endif

    public void InitializeGame()
    {
        WordCatcher();
        WordDecomposition();
        WordLetterRearranger();
        SetDefinition();
    }

    public int RandomNumberGenerator()
    {
        int temptativeIndex;
        temptativeIndex = (int)Random.Range(lowerRNGLimit, upperRNGLimit);

        return temptativeIndex;
    }

    private int IndexDeterminer()
    {
        int index;
        do
        {
            index = RandomNumberGenerator();
        } while (matrixIndexAvailability[index]==false);

        matrixIndexAvailability[index]=false;

        return index;
    }

    public void MissionDataRecieved(FirebaseManager.CallbackResult result,
       MissionData? data, string message) {
        switch(result) {
            case FirebaseManager.CallbackResult.Canceled:
            case FirebaseManager.CallbackResult.Faulted:
            case FirebaseManager.CallbackResult.Invalid:
                Debug.LogError(message);
                break;
            case FirebaseManager.CallbackResult.Success:
            default:
                Debug.Log(message);
                UnityMainThreadDispatcher
                    .Instance ()
                    .Enqueue(WordLoaderWrapper(data));
                break;
        }
    }

    IEnumerator WordLoaderWrapper(MissionData? data) {
        WordLoader(data.Value.Data);
        yield return null;
    }

    private void WordLoader(DocumentStore data) {
        List<string[]> listaMatrix = new List<string[]>();
        List<int> listaNivel = new List<int>();
        List<bool> listaMatrixIndexAvailability = new List<bool>();
        List<object> definitions = data["definitions"] as List<object>;
        for (int i = 1; i < definitions.Count; i++) {
            DocumentStore levels = definitions[i] as DocumentStore;
            foreach (KeyValuePair<string, object> level in levels) {
                string[] pair = new string[2];
                pair[0] = level.Key;
                pair[1] = (string) level.Value;
                listaMatrix.Add(pair);
                listaNivel.Add(i);
                listaMatrixIndexAvailability.Add(true);
            }
        }
        matrix = new string[listaMatrix.Count, 2];
        for(int i = 0; i < listaMatrix.Count; i++) {
            string[] pair = listaMatrix[i];
            for (int j = 0; j < 2; j++) {
                matrix[i, j] = pair[j];
            }
        }
        nivel = listaNivel.ToArray();
        matrixIndexAvailability = listaMatrixIndexAvailability.ToArray();
        InitializeGame();
    }

    private string WordCatcher()
    {
        /*  Necesario guardar el valor obtenido en IndexDeterminer(), ya que llamar la función repetidas
            veces produciría números diferentes con cada iteración.
         */
        savedIndex = IndexDeterminer();
        recoveredVocabWord = matrix[savedIndex, 0];

        Debug.Log("Índice: " + savedIndex + "\nPalabra Recuperada: " + recoveredVocabWord + "\nDefinición Correspondiente: " + matrix[savedIndex, 1]);

        return recoveredVocabWord;
    }

    private void WordDecomposition()
    {
        // ¿Será necesario esta parte? Siento que ya quedaría guardado el String desde el método anterior.
        //recoveredVocabWord = WordCatcher();

        Debug.Log("Palabra original: " + recoveredVocabWord);

        for (int i=0; i<(recoveredVocabWord.Length); i++)
        {
            MovAndSlotObjRef = Instantiate(prefabMovableAndSlot, banco.transform);
            MovAndSlotObjRef.GetComponentInChildren<Text>().text = System.Convert.ToString(recoveredVocabWord[i]).ToUpper();

            SlotObjRef = Instantiate(prefabSlot, recibidor.transform);
            Slot SlotObj = SlotObjRef.GetComponent<Slot>();
            //Debug.Log(SlotObj.slotLetter);    //El problema no es que no pueda llamar a Slot.
            SlotObj.slotLetter = System.Convert.ToString(recoveredVocabWord[i]).ToUpper();
            SlotObj.vocabManager = this;
        }
    }

    private void WordLetterRearranger()
    {
        for (int i=0; i<(recoveredVocabWord.Length); i++)
        {
            banco.GetComponentInChildren<Slot>().GetComponentInChildren<Transform>().SetSiblingIndex(IndexArranger(recoveredVocabWord.Length));
        }
    }

    private void SetDefinition()
    {
        definition.GetComponent<Text>().text = matrix[savedIndex, 1];
    }

    public void endWord()
    {
        DestroyAllRecibidor();
        DestroyAllBanco();
        definition.GetComponent<Text>().text = "";
        InitializeGame();
    }

    public bool dispAvailability()
    {
        bool anyAvailableSpaces = false;

        // Habrá que cambiar ese límite de 30 a 'upperRNGLimit'. E inicializar el índice a partir de 'lowerRNGLimit'.
        for (int i=0; i<30; i++)
        {
            if (matrixIndexAvailability[i] == true)
            {
                anyAvailableSpaces = true;
            }
        }

        return anyAvailableSpaces;
    }

    /* No funciona como quisiera--no siempre regresa números diferentes--pero ha de servir para mis pro-
     * pósitos de revolver los índices de las Objetos Hijos del Banco de Letras.
     */
    public int IndexArranger(int wordLength)
    {
        int newIndex, temp;
        bool [] characterAvailability = new bool[wordLength];
        //bool enableCycle = true;

        for (int i=0; i<wordLength; i++)
        {
            characterAvailability[i] = true;
        }

        do
        {
            temp = (int)(Random.Range(0, wordLength-1));
        }
        while (characterAvailability[temp] == false);

        characterAvailability[temp] = false;
        newIndex = temp;

        return newIndex;
    }

    public void DestroyAllBanco()
    {
        if (hasDestroyedBanco == false)
        {
            foreach (Transform child in banco.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            // Necesita volverse 'false' una vez que se haya completado un nivel.
            hasDestroyedBanco = true;
        }
    }

    public void DestroyAllRecibidor()
    {
        if (hasDestroyedRecibidor == false)
        {
            foreach (Transform child in recibidor.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            // Necesita volverse 'false' una vez que se haya completado un nivel.
            hasDestroyedRecibidor = true;
        }
    }
}
