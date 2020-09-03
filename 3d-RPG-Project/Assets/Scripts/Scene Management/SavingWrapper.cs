using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultFile = "save";

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        private void Load()
        {
            GetComponent<SavingSystem>().Load(defaultFile);
        }

        private void Save()
        {
            GetComponent<SavingSystem>().Save(defaultFile);
        }
    }

}