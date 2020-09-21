﻿using Deviax.QueryBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NSSLServer.Models
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public virtual List<ListItem> Products { get; set; }
        public virtual List<Contributor> Contributors { get; set; }
        public ShoppingList() { }
        public ShoppingList(User c, List<ListItem> p, string name)
        {
            Products = p;
            //Contributors.Add(c);
            Name = name;
        }

        public static ShoppingListTable T = new ShoppingListTable("sl");
        [PrimaryKey(nameof(Id))]
        public class ShoppingListTable : Table<ShoppingListTable>
        {
            public Field Id;
            public Field Name;

            public ShoppingListTable(string tableAlias = null) : base("public","shoppinglists" , tableAlias)
            {
                Id = F("id");
                Name = F("name");
            }
        }
    }

   
}