using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour
{
    public static int totalFruits;

    public Text UIText;
    // Public access due to reference in Animation.cs
    public /*static*/ string[] sentences, maleNounsString, femaleNounsString;
    public /*static*/ int index;
    public GameObject continueButton, gobackButton, dialogBackground, endgame, skip, loader;
    public FruitInitialization initializer;

    private GameObject inicio;
    public GameObject  handanimation = null;
    public GameObject  repetir = null;
    private Image myImage;
    int sceneIndex;
    [SerializeField] float typingSpeed;
    [SerializeField] AudioClip[] audios, maleNounsFeru, femaleNounsFeru, maleNounsMati, femaleNounsMati, un, numbersFeru, numbersMati, pluralFruitsFeru, pluralFruitsMati, singleFruitsFeru, singleFruitsMati;
    AudioSource source, individualSource;
    bool continuarpresionado = true;

    public GameObject Manzana1, Manzana2, Durazno1, Durazno2, Pera;

    /* More convenient to track the Slider's value from SliderNumber's printValue() than from the Slider's attribute it-
       self, given that printValue() is the method in charge of printing the slider's value each time it has been modi-
       ied, and trying to register the slider's value from the Slider would require being part of the Update() method.
       Less efficient.
     */

    void Start()
    {
        /* Éste es , Éstos son
        maleNounsString = new string[2];
        // Ésta es , Éstas son
        femaleNounsString = new string[2];

        // Éste es, Éstos son
        maleNounsFeru = new AudioClip[2];
        maleNounsMati = new AudioClip[2];

        // Ésta es, Éstas son
        femaleNounsFeru = new AudioClip[2];
        femaleNounsMati = new AudioClip[2];

        // Dicho por Feru, Mati
        un = new AudioClip[2];

        // 1 (una), 2, ..., 10
        numbersFeru = new AudioClip[10];
        numbersMati = new AudioClip[10];

        // Manzana, Pera, Durazno
        singleFruitsFeru = new AudioClip[3];
        singleFruitsMati = new AudioClip[3];

        // Manzanas, Peras, Duraznos
        pluralFruitsFeru = new AudioClip[3];
        pluralFruitsMati = new AudioClip[3];*/
        
        typingSpeed = 0.02f;

        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        dialogBackground.SetActive(true);
        continueButton.SetActive(false);
        gobackButton.SetActive(false);
        skip.SetActive(false);
        

        if (SceneManager.GetActiveScene().name == "Tutorial" && index == 0){
           handanimation =  GameObject.FindGameObjectWithTag("Hand");
           handanimation.SetActive(false);
           Pera.SetActive(false);
                Manzana2.SetActive(false);
                Manzana1.SetActive(false);
                Durazno1.SetActive(false);
                Durazno2.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "7Game_Intro" && index == 0){
           inicio =  GameObject.FindGameObjectWithTag("Inicio");
           inicio.SetActive(false);
        }

        source = GetComponent<AudioSource>();
        individualSource = new AudioSource();

        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        if (SceneManager.GetActiveScene().name == "New Corrección")
        {
            switch(index)
            {
                // Feru
                case 0:
                    // Fruta (singular)
                    if (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]] == 1)
                    {
                        // Masculina (Durazno)
                        if (initializer.fruits[FruitInitialization.fruitIndexes[0]].name == "Durazno")
                        {
                            // Éste es
                            source.PlayOneShot(maleNounsFeru[0]);
                            foreach(char letter in maleNounsString[0].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // un

                            source.PlayOneShot(un[0]);
                            foreach(char letter in "1 ".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // Durazno
                            source.PlayOneShot(singleFruitsFeru[2]);
                            foreach(char letter in "Durazno.".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }

                        // Femenina (Manzana, Pera)
                        else
                        {
                            // Ésta es
                            source.PlayOneShot(femaleNounsFeru[0]);
                            foreach(char letter in femaleNounsString[0].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // una
                            source.PlayOneShot(numbersFeru[0]);
                            foreach(char letter in "1 ".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (fruta)
                            source.PlayOneShot(singleFruitsFeru[FruitInitialization.fruitIndexes[0]]);
                            foreach(char letter in (initializer.fruits[FruitInitialization.fruitIndexes[0]].name + ".").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }
                    }

                    // Frutas (plural)
                    else
                    {
                        // Masculinas (Duraznos)
                        if (initializer.fruits[FruitInitialization.fruitIndexes[0]].name == "Durazno")
                        {
                            // Éstos son
                            source.PlayOneShot(maleNounsFeru[1]);
                            foreach(char letter in maleNounsString[1].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (número variable)
                            source.PlayOneShot(numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]]-1]);
                            foreach(char letter in (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]].ToString() + " ").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // Duraznos
                            source.PlayOneShot(pluralFruitsFeru[2]);
                            foreach(char letter in "Duraznos.".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }

                        // Femeninas (Manzanas, Peras)
                        else
                        {
                            // Éstas son
                            source.PlayOneShot(femaleNounsFeru[1]);
                            foreach(char letter in femaleNounsString[1].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (número variable)
                            source.PlayOneShot(numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]]-1]);
                            foreach(char letter in (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]].ToString() + " ").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (frutas)
                            source.PlayOneShot(pluralFruitsFeru[FruitInitialization.fruitIndexes[0]]);
                            foreach(char letter in (initializer.fruits[FruitInitialization.fruitIndexes[0]].name + "s.").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }
                    }

                    /*foreach(char letter in sentences[index].ToCharArray())
                    {
                        UIText.text += letter;
                        yield return new WaitForSeconds(typingSpeed);
                    }

                    foreach(char letter in (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]].ToCharArray())
                    {
                        UIText.text += letter;
                        yield return new WaitForSeconds(typingSpeed);
                    }*/

                    break;

                // Mati
                case 1:
                    // Fruta (singular)
                    if (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]] == 1)
                    {
                        // Masculina (Durazno)
                        if (initializer.fruits[FruitInitialization.fruitIndexes[1]].name == "Durazno")
                        {
                            // Éste es
                            source.PlayOneShot(maleNounsMati[0]);
                            foreach(char letter in maleNounsString[0].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // un
                            source.PlayOneShot(un[1]);
                            foreach(char letter in "1 ".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // Durazno
                            source.PlayOneShot(singleFruitsMati[2]);
                            foreach(char letter in "Durazno.".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }

                        // Femenina (Manzana, Pera)
                        else
                        {
                            // Ésta es
                            source.PlayOneShot(femaleNounsMati[0]);
                            foreach(char letter in femaleNounsString[0].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // una
                            source.PlayOneShot(numbersMati[0]);
                            foreach(char letter in "1 ".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (fruta)
                            source.PlayOneShot(singleFruitsMati[FruitInitialization.fruitIndexes[1]]);
                            foreach(char letter in (initializer.fruits[FruitInitialization.fruitIndexes[1]].name + ".").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }
                    }

                    // Frutas (plural)
                    else
                    {
                        // Masculinas (Duraznos)
                        if (initializer.fruits[FruitInitialization.fruitIndexes[1]].name == "Durazno")
                        {
                            // Éstos son
                            source.PlayOneShot(maleNounsMati[1]);
                            foreach(char letter in maleNounsString[1].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (número variable)
                            source.PlayOneShot(numbersMati[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]]-1]);
                            foreach(char letter in (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]].ToString() + " ").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // Duraznos
                            source.PlayOneShot(pluralFruitsMati[2]);
                            foreach(char letter in "Duraznos.".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }

                        // Femeninas (Manzanas, Peras)
                        else
                        {
                            // Éstas son
                            source.PlayOneShot(femaleNounsMati[1]);
                            foreach(char letter in femaleNounsString[1].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (número variable)
                            source.PlayOneShot(numbersMati[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]]-1]);
                            foreach(char letter in (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]].ToString() + " ").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (frutas)
                            source.PlayOneShot(pluralFruitsMati[FruitInitialization.fruitIndexes[1]]);
                            foreach(char letter in (initializer.fruits[FruitInitialization.fruitIndexes[1]].name + "s.").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }
                    }

                    break;

                // Feru
                case 2:
                    // Fruta (singular)
                    if (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]] == 1)
                    {
                        // Masculina (Durazno)
                        if (initializer.fruits[FruitInitialization.fruitIndexes[2]].name == "Durazno")
                        {
                            // Éste es
                            source.PlayOneShot(maleNounsFeru[0]);
                            foreach(char letter in maleNounsString[0].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // un
                            source.PlayOneShot(un[0]);
                            foreach(char letter in "1 ".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // Durazno
                            source.PlayOneShot(singleFruitsFeru[2]);
                            foreach(char letter in "Durazno.".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }

                        // Femenina (Manzana, Pera)
                        else
                        {
                            // Ésta es
                            source.PlayOneShot(femaleNounsFeru[0]);
                            foreach(char letter in femaleNounsString[0].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // una
                            source.PlayOneShot(numbersFeru[0]);
                            foreach(char letter in "1 ".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (fruta)
                            source.PlayOneShot(singleFruitsFeru[FruitInitialization.fruitIndexes[2]]);
                            foreach(char letter in (initializer.fruits[FruitInitialization.fruitIndexes[2]].name + ".").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }
                    }

                    // Frutas (plural)
                    else
                    {
                        // Masculinas (Duraznos)
                        if (initializer.fruits[FruitInitialization.fruitIndexes[2]].name == "Durazno")
                        {
                            // Éstos son
                            source.PlayOneShot(maleNounsFeru[1]);
                            foreach(char letter in maleNounsString[1].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (número variable)
                            source.PlayOneShot(numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]]-1]);
                            foreach(char letter in (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]].ToString() + " ").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // Duraznos
                            source.PlayOneShot(pluralFruitsFeru[2]);
                            foreach(char letter in "Duraznos.".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }

                        // Femeninas (Manzanas, Peras)
                        else
                        {
                            // Éstas son
                            source.PlayOneShot(femaleNounsFeru[1]);
                            foreach(char letter in femaleNounsString[1].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (número variable)
                            source.PlayOneShot(numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]]-1]);
                            foreach(char letter in (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]].ToString() + " ").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (frutas)
                            source.PlayOneShot(pluralFruitsFeru[FruitInitialization.fruitIndexes[2]]);
                            foreach(char letter in (initializer.fruits[FruitInitialization.fruitIndexes[2]].name + "s.").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }
                    }

                    break;

                // Mati
                case 3:
                    // "Juntas dan..."
                    source.PlayOneShot(audios[0]);
                    foreach(char letter in sentences[0].ToCharArray())
                    {
                        UIText.text += letter;
                        yield return new WaitForSeconds(typingSpeed);
                    }

                    // (total de frutas)
                    source.PlayOneShot(audios[0]);
                    foreach(char letter in ((FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]] + FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]] + FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]]).ToString() + " ").ToCharArray())
                    {
                        UIText.text += letter;
                        yield return new WaitForSeconds(typingSpeed);
                    }

                    // "...frutas."
                    source.PlayOneShot(audios[1]);
                    foreach(char letter in sentences[1].ToCharArray())
                    {
                        UIText.text += letter;
                        yield return new WaitForSeconds(typingSpeed);
                    }
                    break;
            }

            continueButton.SetActive(true);
            /*source.PlayOneShot(audios[index]);

            if (index == 0 || index == 2)
            {
                source.PlayOneShot(audios[0]);
                source.PlayOneShot((numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]]]) + 1);
                source.PlayOneShot((pluralFruitsFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]]]) + 1);
            }*/
        }

        else
        {
            source.PlayOneShot(audios[index]);

            foreach(char letter in sentences[index].ToCharArray())
            {
                UIText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            if (SceneManager.GetActiveScene().name == "7Game_Intro" && index == 1){
                inicio.SetActive(true);
                continueButton.SetActive(false);
                gobackButton.SetActive(true);
            }
            else if (sceneIndex >= 14 && sceneIndex <= 20)
            {
                if (sceneIndex < 20) skip.SetActive(true);
                continueButton.SetActive(true);
                if (sceneIndex > 14) gobackButton.SetActive(true);
            }
            else if(SceneManager.GetActiveScene().name == "Tutorial"){
                if(index == 0){
                    skip.SetActive(true);
                    continueButton.SetActive(true);
                }
                else if(index == 2){
                   repetir.SetActive(true);
                   continueButton.SetActive(true); 
                }
                else
                continueButton.SetActive(true);
            }
            else {
                continueButton.SetActive(true);
            }
        }
    }

    // Public due to it being referenced by Dialog Background's ContinueButton.
    public void NextSentence()
    {
        continueButton.SetActive(false);
        gobackButton.SetActive(false);
        skip.SetActive(false);

        if (SceneManager.GetActiveScene().name == "7Game_Intro" && index == 1){
            inicio.SetActive(false);
        }
        if (SceneManager.GetActiveScene().name == "Tutorial" && index == 1){ // Esto solo sucede en el tutorial de matemáticas de conteo
                
                
                Pera.SetActive(true);
                Manzana2.SetActive(true);
                Manzana1.SetActive(true);
                Durazno1.SetActive(true);
                Durazno2.SetActive(true);
                source.enabled = false;
                /*myImage = dialogBackground.GetComponent<Image>();
                UIText.enabled = false;
                myImage.enabled = false;*/
                handanimation.SetActive(true);
                dialogBackground.SetActive(false);
                
                
        }
        loader= GameObject.FindWithTag("Cross_Fade");
        if (SceneManager.GetActiveScene().name != "New Corrección")
        {
            if (index < sentences.Length - 1)
            {
                index++;
                UIText.text = "";
                StartCoroutine(Type());
            }
            else
            {
                UIText.text = "";
                dialogBackground.SetActive(false);

                if (SceneManager.GetActiveScene().name == "Agradecimiento" || SceneManager.GetActiveScene().name == "New Corrección")
                    endgame.SetActive(true);
                else if(SceneManager.GetActiveScene().name == "Tutorial" && index == 2){
                    SceneManager.LoadScene("Juego 1");
                }
                else if (loader != null && loader.GetComponent<Levelloader>() != null)
                    loader.GetComponent<Levelloader>().LoadNextLevel(continuarpresionado);
                else
                    SceneManager.LoadScene(sceneIndex+1);
            }
        }
        else
        {
            if (index < 3)
            {
                index++;
                UIText.text = "";
                StartCoroutine(Type());
            }
            else
            {
                UIText.text = "";
                dialogBackground.SetActive(false);

                if (SceneManager.GetActiveScene().name == "Agradecimiento" || SceneManager.GetActiveScene().name == "New Corrección")
                    endgame.SetActive(true);
                else
                    SceneManager.LoadScene(sceneIndex+1);
            }
        }
    }
    public void PrevSentence()
    {
        continueButton.SetActive(false);
        gobackButton.SetActive(false);
        if (SceneManager.GetActiveScene().name == "7Game_Intro" && index == 1){
            inicio.SetActive(false);
        }
        continuarpresionado = false;
        skip.SetActive(false);
        loader= GameObject.FindWithTag("Cross_Fade");
        if (index >0)
        {
            index--;
            UIText.text = "";
            StartCoroutine(Type());
        }
        else
        {
            UIText.text = "";
            dialogBackground.SetActive(false);

            if (SceneManager.GetActiveScene().name == "Agradecimiento" || SceneManager.GetActiveScene().name == "New Corrección")
                endgame.SetActive(true);
            else if (loader != null && loader.GetComponent<Levelloader>() != null)
                loader.GetComponent<Levelloader>().LoadNextLevel(continuarpresionado);
            else
                SceneManager.LoadScene(sceneIndex-1);
        }
    }
    public void notstart(){ //Used for skiping tutorials or intros.
        if(SceneManager.GetActiveScene().name == "Tutorial"){
            SceneManager.LoadScene("Juego 1");
        }
        else{
            SceneManager.LoadScene("7Game_Intro");
        }
    }

    public void repitScene(){
        SceneManager.LoadScene("Tutorial");
    }

    /*void Update() {
        if(handanimation.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
                    Debug.Log("not playing");
                    myImage.enabled = true;
                    UIText.enabled = true;
                    continueButton.SetActive(true);
                    handanimation.SetActive(false);
        }
        else{
                    Debug.Log("playing");
        } 
    }*/
}
