using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Resource Class
    private class Resource
    {
        private string resourceName;
        private float resourceAmount;
        
        public Resource([NotNull] string resourceName, float resourceAmount)
        {
            this.resourceName = resourceName ?? throw new ArgumentNullException(nameof(resourceName));
            this.resourceAmount = resourceAmount;
        }

        public string getName()
        {
            return resourceName;
        }

        public float getAmount()
        {
            return resourceAmount;
        }

        public void addResource(float income)
        {
            resourceAmount += income;
        }
    }
    #endregion

}
