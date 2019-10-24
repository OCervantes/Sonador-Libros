using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class VocabularyManager: MonoBehaviour
{
    //Datos Miembro
    public AudioSource bounceSource, lockSource;
    public GameObject recibidor, banco, definition,
      prefabMovableAndSlot = null, prefabSlot = null, cameraObject = null;
    public bool butDoesItSave = false;

    GameObject MovAndSlotObjRef, SlotObjRef;
    List<string> letrasPal = new List<string>();
    bool hasDestroyedRecibidor = false, hasDestroyedBanco = false;
    int savedIndex;
    string recoveredVocabWord;

    /* Lo tuve que volver estático porque no me permitía de otra manera.
     * Tendré que revisar que no interfiera más tarde.
     */
     /* Esta vez, upperRNGLimit es 3 posiciones superior a lowerRNGLimit, porque al parecer el método Random.
      * Range() es inclusivo en cuanto a su límite inferior, pero exclusivo para el superior.
      */
    static int lowerRNGLimit = 0, upperRNGLimit = lowerRNGLimit+3;
    /* Declaración e Inicialización de la Matriz
     * Palabra, Definición
     * 30 elementos--3 elementos por nivel; 10 niveles.
     */
    public string[,] matrix = { {"Mugido", "Voz del ganado vacuno."},
                         {"Abatir", "Derribar, bajar, tumbar."},
                         {"Ancas", "Cada una de las dos mitades laterales de la parte posterior de algunos animales."},
                         {"Reacio", "Contrario a algo."},
                         {"Resplandor", "Luz muy clara que arroja o despide el Sol u otro cuerpo luminoso."},
                         {"Faena", "Trabajo que requiere un esfuerzo mental o físico."},
                         {"Anaquel", "Estante de un armario, librería, alacena, etc."},
                         {"Mágico", "De la magia o relacionado con ella."},
                         {"Mecánico", "De la mecánica o relacionado con ella."},
                         {"Nácar", "Sustancia dura, blanca, irisada que se forma en el interior de las conchas de algunos moluscos y que produce brillos y tonos de distintos colores cuando refleja la luz."},

                         {"Conjunción", "Junta, unión."},
                         {"Vaivén", "Balanceo, movimiento alternativo y sucesivo de un lado a otro."},
                         {"Retozar", "Saltar y brincar alegremente."},
                         {"Terciopelo", "Tela de seda muy tupida y con pelo, formada por dos urdimbres y una trama."},
                         {"Mozo", "De la juventud o relativo a ella."},
                         {"Extirpar", "Arrancar de cuajo o de raíz."},
                         {"Paragüero", "Recipiente generalmente cilíndrico, parecido a un cubo, para dejar los paraguas."},
                         {"Garrapata", "Ácaro de cuerpo oval de unos 6 mm de longitud que vive parásito sobre la piel de perros, aves y otros animales, incluido el ser humano."},
                         {"Guiñar", "Cerrar y abrir con rapidez un ojo dejando el otro abierto, generalmente para hacer una señal."},
                         {"Guirnalda", "Adorno consistente en una tira entretejida de flores y ramas que se coloca en forma de corona o de ondas."},

                         {"Jilguero", "Pájaro cantor de unos 12 cm de longitud, pico grueso y puntiagudo, y plumaje pardo en el dorso, con la cara roja y blanca, la cola negra, las alas negras y amarillas, y una mancha negra sobre la cabeza."},
                         {"Banquillo", "Asiento que ocupa el procesado ante el tribunal durante un juicio."},
                         {"Chirrió", "Dar sonido agudo ciertas cosas cuando son manipuladas, como el tocino cuando se fríe."},
                         {"Complicidad", "Participación de una persona junto con otras en la comisión de un delito o colaboración en él sin tomar parte en su ejecución material."},
                         {"Moraleja", "Enseñanza que se deduce de algo, especialmente de un cuento o de una fábula." },
                         {"Gentilicio", "Que expresa el origen geográfico o racial." },
                         {"Cósmico", "Del cosmos o relacionado con él." },
                         {"Úlcera", "Llaga o lesión que aparece en la piel o en el tejido de las mucosas a causa de una pérdida de sustancia y que no tiende a la cicatrización." },
                         {"Codicia", "Deseo vehemente de poseer muchas cosas, especialmente riquezas o bienes." },
                         {"Escepticismo", "Recelo, incredulidad o falta de confianza en la verdad o eficacia de una cosa." }
                       };

    /* Arreglo de enteros. Posición del entero proporcional a la palabra a aparecer en aquel mismo nivel.
     * Contiene 30 enteros, equivalente a las 30 palabras almacenadas en la matriz.
     * Entero almacenado corresponde al nivel de la palabra.
     */
    public int[] nivel = { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 10};
    // Arreglos necesitan ser 'private'
    private bool[] matrixIndexAvailability = new bool[30]; //= { true, true, true, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 10 };
    //Métodos
    public void Start()
    {
        /* Inicialización del arreglo 'matrixIndexAvailability'.
         * Resulta que una matrix tiene una longitud correspondiente al número completo de elementos almacenados--no el de columnas/filas, como tenía pensado.
         * Va fuera del método 'inititateNewWord()', ya que, si no lo fuese, al momento de ejecutarlo, el arreglo de 'matrixIndexAvailability[]' nunca actualizaría el valor de sus contenidos.
         */
        for (int i=0; i<((matrix.Length)/2); i++)
        {
            matrixIndexAvailability[i] = true;
        }

        WordCatcher();
        WordDecomposition();
        WordLetterRearranger();
        SetDefinition();
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

    public void initiateWord()
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

    private int IndexDeterminer(/* int loops*/)
    {
        int index;
        do
        {
            index = RandomNumberGenerator();
        } while (matrixIndexAvailability[index]==false);

        matrixIndexAvailability[index]=false;

        return index;
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
