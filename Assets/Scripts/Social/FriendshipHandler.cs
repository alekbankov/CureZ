using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendshipHandler : MonoBehaviour
{
    class Friend : IComparable
    {
        public string name;
        public int level;
        public Friend(string name, int level)
        {
            this.name = name;
            this.level = level;
        }
		
        public int CompareTo(object obj) {
            if (obj == null) return 1;

            Friend otherFriend = obj as Friend;
            if (otherFriend != null)
                return otherFriend.level.CompareTo(this.level);
            else
                throw new ArgumentException("Object is not a Friend");
        }
    }
        
    void AddFriend()
    {
        
    }
}
