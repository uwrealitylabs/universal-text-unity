using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UniversalTextScanner : MonoBehaviour
{
    //to-do: impl. methods of search for different search points

    public class SearchPoint : StringAggregator
    {
        private string _description; //desc of search point, e.g. "The user is pointing at"
        private Func<List<UniversalTextTag>> _search;

        public SearchPoint(string description, Func<List<UniversalTextTag>> search)
        {
            this._description = description;
            this._search = search;
        }

        public List<UniversalTextTag> Search()
        {
            return _search();
        }

        protected override string Aggregate()
        {
            List<UniversalTextTag> tags = Search();
            string representation = "";
            int index = 0;
            foreach (UniversalTextTag tag in tags)
            {
                tagRep = tag.Representation;
                if (index > 0)
                {
                    representation += "Also, " + char.ToLower(_description[0]) + _description.Substring(1);
                }
                else
                {
                    representation += _description;
                }
                representation += char.ToLower(tagRep[0]) + tagRep.Substring(1) + " ";
                index++;
            }
            /*
             * ex {"An apple on top of a plate.", "A pencil on the table"} => "The user is pointing at an apple on top of a plate. Also, 
             * the user is pointing at a pencil on the table, which is 60 cm by 90 cm."
             */
            return representation;
        }
    }
}

