using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RIT_RPG
{
    // linked list of playable characters; each spot in the list is a "row"
    class CharacterRows
    {
        // attributes
        private Character head;
        private int count;

        // properties
        public Character Head
        {
            get { return head; }
        }

        public int Count
        {
            get { return count; }
        }

        // constructor
        public CharacterRows()
        {
            head = null;
            count = 0;
        }

        // add a new character to the list
        public void Add(Character ch)
        {
            {
                // create a new character
                Character newCharacter = ch;
                newCharacter.Next = null;

                // special case - no characters in the list
                if (head == null)
                {
                    head = newCharacter;
                    count++;
                    return;
                }

                // there is at least 1 character and possibly more
                // so step through the list to find the last one
                Character current = head; // at start of the list
                while (current.Next != null) // connected to another character
                {
                    current = current.Next;
                }

                // current is now the last character
                current.Next = newCharacter;
                count++;
            }
        }

        // method to insert character into middle of rows
        public void Insert(Character ch, int index)
        {
            // create a new node
            Character newCharacter2 = ch;
            newCharacter2.Next = null;

            // new head of the list
            if (index < 0)
            {

                newCharacter2.Next = head; // link to existing head of list
                head = newCharacter2; // new node is now the head of the list
                count++;
                return;
            }

            // at end of list
            if (index > count)
            {
                Add(newCharacter2);
                return;
            }
            // all other cases covered, last case - insert into the middle
            // of the list

            // variables to show current and previous nodes
            Character prev = head;
            Character curr = head;

            for (int i = 0; i < count; i++)
            {
                // at right location
                if (i == index)
                {
                    // connect the new character to the current one
                    newCharacter2.Next = curr;

                    // connect previous node to the new node
                    prev.Next = newCharacter2;
                    count++; // another node in the list
                    return;
                }

                prev = curr; // move previous node up one
                curr = curr.Next; // move the current node up one also
            }
        }


        // delete a character
        public Character Delete(string str)
        {
            // variables to show current and previous nodes
            Character prev = head;
            Character curr = head;

            for (int i = 0; i < count; i++)
            {
                // at right location
                if (curr.Name == str)
                {
                    if (i == 0) // if the right location is at the head
                    {
                        head = head.Next;
                        count--;
                        return curr;
                    }

                    else
                    {
                        // connect the previous node to the current one's next node
                        prev.Next = curr.Next;

                        count--; // remove the node in the list
                        break;
                    }
                }

                prev = curr; // move previous node up one
                curr = curr.Next; // move the current node up one also
            }

            return curr;
        }

        // this method checks to see if this character's name is the same as the one in the row selected
        public bool CheckForDuplicate(string str, int ind)
        {
            Character current = head;
            int num = 0;
            if (ind < 0 || ind >= count) // special case - index is less than zero or greater than or equal to the count
            {
                return false; // return nothing
            }

            // step through the data until the correct node is found
            while (ind != num)
            {
                current = current.Next;
                num++;
            }

            if( str == current.Name )
            {
                return true;
            }

            return false;
        }
    }
}
