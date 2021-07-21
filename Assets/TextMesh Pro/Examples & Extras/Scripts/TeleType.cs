using System.Collections;
using UnityEngine;


namespace TMPro.Examples
{

    public class TeleType : MonoBehaviour
    {
        [Range(1, 100)]
        public int RevealSpeed = 50;

        private TMP_Text m_textMeshPro;


        void Awake()
        {
            // Get Reference to TextMeshPro Component
            m_textMeshPro = GetComponent<TMP_Text>();
        }


        IEnumerator Start()
        {

            // Force and update of the mesh to get valid information.
            m_textMeshPro.ForceMeshUpdate();


            int totalVisibleCharacters = m_textMeshPro.textInfo.characterCount; // Get # of Visible Character in text object
            int counter = 0;
            while (true)
            {
                int visibleCount = counter % (totalVisibleCharacters + 1);
                m_textMeshPro.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

                // Once the last character has been revealed, wait 1.0 second and start over.
                if (visibleCount >= totalVisibleCharacters)
                {
                    yield return new WaitForSeconds(Random.Range(0.5f, 2.0f));
                    counter = -1;
                }

                counter += 1;

                yield return new WaitForSeconds(1f / RevealSpeed);
            }
        }

    }
}