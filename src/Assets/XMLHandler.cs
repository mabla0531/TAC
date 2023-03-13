using System.Xml;
using SFML.Graphics;
using System.Collections.Generic;

namespace TAC {

    class Node {
        public string Name {get; set;}
        public List<string> Attributes {get; set;} = new List<string>();
        public List<Node> Subnodes {get; set;} = new List<Node>();
    }

    static class XMLHandler {
        public static void writeItem(XmlWriter writer, Item i, bool element = true) {
            if (element)
                writer.WriteStartElement("Item");
            
            writer.WriteAttributeString("Weight", i.Weight.ToString());
            writer.WriteAttributeString("Value", i.Value.ToString());
            writer.WriteAttributeString("Name", i.Name);
            writer.WriteAttributeString("Attack", i.Attack.ToString());
            writer.WriteAttributeString("Defense", i.Defense.ToString());
            writer.WriteAttributeString("Health", i.Health.ToString());
            writer.WriteAttributeString("Stamina", i.Stamina.ToString());
            writer.WriteAttributeString("Sprite", i.Icon.TextureRect.Left + " " + i.Icon.TextureRect.Top + " " + i.Icon.TextureRect.Width + " " + i.Icon.TextureRect.Height);
            writer.WriteAttributeString("Rarity", i.ItemRarity.ToString());
            writer.WriteAttributeString("Slot", i.ItemSlot.ToString());
            
            if (element)
                writer.WriteEndElement();
        }

        public static void writeSaveFile(GameState gameState) {
            XmlTextWriter writer = new XmlTextWriter("saves/test.tac", System.Text.Encoding.UTF8);
            writer.WriteStartDocument(true);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 4;
            writer.WriteStartElement("Save");

            //begin player node
            writer.WriteStartElement("Player");
            writer.WriteAttributeString("X", gameState.player.X.ToString());
            writer.WriteAttributeString("Y", gameState.player.Y.ToString());
            writer.WriteAttributeString("MaxHealth", gameState.player.MaxHealth.ToString());
            writer.WriteAttributeString("Health", gameState.player.Health.ToString());
            writer.WriteAttributeString("MaxStamina", gameState.player.MaxStamina.ToString());
            writer.WriteAttributeString("Stamina", gameState.player.Stamina.ToString());
            writer.WriteStartElement("Hand");
            if (gameState.player.Hand != null) writeItem(writer, gameState.player.Hand, false);
            writer.WriteEndElement();
            writer.WriteStartElement("Offhand");
            if (gameState.player.Offhand != null) writeItem(writer, gameState.player.Offhand, false);
            writer.WriteEndElement();
            writer.WriteStartElement("Head");
            if (gameState.player.Head != null) writeItem(writer, gameState.player.Head, false);
            writer.WriteEndElement();
            writer.WriteStartElement("Chest");
            if (gameState.player.Chest != null) writeItem(writer, gameState.player.Chest, false);
            writer.WriteEndElement();
            writer.WriteStartElement("Legs");
            if (gameState.player.Legs != null) writeItem(writer, gameState.player.Legs, false);
            writer.WriteEndElement();
            writer.WriteStartElement("Feet");
            if (gameState.player.Feet != null) writeItem(writer, gameState.player.Feet, false);
            writer.WriteEndElement();

            foreach (Item i in gameState.player.inventory.Items) {
                writeItem(writer, i);
            }

            writer.WriteEndElement();
            //end player node
            
            //begin map nodes
            foreach (Map map in gameState.Maps) {
                writer.WriteStartElement("Map");
                writer.WriteAttributeString("Name", map.MapName); //TODO: make the map hold entities
                
                //begin entity nodes
                writer.WriteStartElement("Entities");
                foreach(Entity e in map.Entities) {
                    if (e == gameState.player) //don't write player to save file as an entity
                        continue;

                    string type = e.GetType().ToString().Substring(4); //get type without namespace "TAC."
                    writer.WriteStartElement(type);
                    writer.WriteAttributeString("X", e.X.ToString());
                    writer.WriteAttributeString("Y", e.Y.ToString());
                    writer.WriteAttributeString("MaxHealth", e.MaxHealth.ToString());
                    writer.WriteAttributeString("Health", e.Health.ToString());

                    foreach (Item i in e.inventory.Items) {
                        writeItem(writer, i);
                    }

                    writer.WriteEndElement();
                }
                //grounditems are stored in a separate list, but are still entities
                foreach (GroundItem g in map.Items) {

                    writer.WriteStartElement("GroundItem");
                    writer.WriteAttributeString("X", g.X.ToString());
                    writer.WriteAttributeString("Y", g.Y.ToString());
                    Item i = g.ItemReference;
                    writer.WriteAttributeString("Weight",  i.Weight.ToString());
                    writer.WriteAttributeString("Value",   i.Value.ToString());
                    writer.WriteAttributeString("Name",    i.Name);
                    writer.WriteAttributeString("Attack",  i.Attack.ToString());
                    writer.WriteAttributeString("Defense", i.Defense.ToString());
                    writer.WriteAttributeString("Health",  i.Health.ToString());
                    writer.WriteAttributeString("Stamina", i.Stamina.ToString());
                    writer.WriteAttributeString("Sprite",  i.Icon.TextureRect.Left + " " + i.Icon.TextureRect.Top + " " + i.Icon.TextureRect.Width + " " + i.Icon.TextureRect.Height);
                    writer.WriteAttributeString("Rarity",  i.ItemRarity.ToString());
                    writer.WriteAttributeString("Slot",    i.ItemSlot.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                //end entity nodes
                writer.WriteEndElement();
            }
            //end map node

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
    
        public static Node readNode(XmlReader reader) {
            Node node = new Node();
            node.Name = reader.Name;

            for (int i = 0; i < reader.AttributeCount; i++) {
                node.Attributes.Add(reader.GetAttribute(i));
            }

            if (reader.IsEmptyElement)
                return node;
            
            //read subnodes
            while (reader.NodeType != XmlNodeType.EndElement) {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element) {
                    node.Subnodes.Add(readNode(reader));
                }
            }
            
            return node;
        }

        public static Item parseItem(List<string> attributes) {
            float weight = float.Parse(attributes[0]);
            int value = int.Parse(attributes[1]);
            string name = attributes[2];
            int attack = int.Parse(attributes[3]);
            int defense = int.Parse(attributes[4]);
            int health = int.Parse(attributes[5]);
            int stamina = int.Parse(attributes[6]);

            //HELL
            IntRect spriteRect = new IntRect();
            string[] spriteRectDimensions = attributes[7].Split(" ");
            spriteRect.Left   = int.Parse(spriteRectDimensions[0]);
            spriteRect.Top    = int.Parse(spriteRectDimensions[1]);
            spriteRect.Width  = int.Parse(spriteRectDimensions[2]);
            spriteRect.Height = int.Parse(spriteRectDimensions[3]);

            Item.Rarity rarity = attributes[8] switch {
                "Unique"    => Item.Rarity.Unique,
                "Legendary" => Item.Rarity.Legendary,
                "Epic"      => Item.Rarity.Epic,
                "Uncommon"  => Item.Rarity.Uncommon,
                "Common"    => Item.Rarity.Common,
                "Useless"   => Item.Rarity.Useless,
                _ => Item.Rarity.Common
            };

            Item.Slot slot = attributes[9] switch {
                "Hand"    => Item.Slot.Hand,
                "Offhand" => Item.Slot.Offhand,
                "Head"    => Item.Slot.Head,
                "Chest"   => Item.Slot.Chest,
                "Legs"    => Item.Slot.Legs,
                "Feet"    => Item.Slot.Feet,
                _ => Item.Slot.Hand
            };

            return new Item(weight, value, name, attack, defense, health, stamina, spriteRect, rarity, slot);
        }

        public static void readSaveFile(GameState gameState) {
            XmlReader reader = XmlReader.Create("saves/test.tac");
            List<Node> nodes = new List<Node>();
            while (reader.Read()) {
                if (reader.NodeType == XmlNodeType.Element && reader.Name != "Save") {
                    nodes.Add(readNode(reader));
                }
            }

            foreach (Node node in nodes) {
                if (node.Name == "Player") {
                    gameState.player.X = float.Parse(node.Attributes[0]);
                    gameState.player.Y = float.Parse(node.Attributes[1]);
                    gameState.player.MaxHealth = int.Parse(node.Attributes[2]);
                    gameState.player.Health = int.Parse(node.Attributes[3]);
                    gameState.player.MaxStamina = int.Parse(node.Attributes[4]);
                    gameState.player.Stamina = float.Parse(node.Attributes[5]);
                
                    foreach (Node n in node.Subnodes) {
                        if (n.Name == "Item")
                            gameState.player.inventory.Items.Add(parseItem(n.Attributes));

                        if (n.Attributes.Count > 0 && System.Array.IndexOf(new string[] {"Hand", "Offhand", "Head", "Chest", "Legs", "Feet"}, n.Name) != -1) {
                            Item i = parseItem(n.Attributes);
                            i.Equipped = true;
                            if (n.Name == "Hand")
                                gameState.player.Hand = i;
                            if (n.Name == "Offhand")
                                gameState.player.Offhand = i;
                            if (n.Name == "Head")
                                gameState.player.Head = i;
                            if (n.Name == "Chest")
                                gameState.player.Chest = i;
                            if (n.Name == "Legs")
                                gameState.player.Legs = i;
                            if (n.Name == "Feet")
                                gameState.player.Feet = i;
                            gameState.player.inventory.Items.Add(i);
                        }
                    }
                }
                if (node.Name == "Map") {
                    Map map = new Map(node.Attributes[0]);
                    foreach (Node n in node.Subnodes) {
                        if (n.Name == "Entities") {
                            foreach (Node entity in n.Subnodes) {
                                if (entity.Name == "HostileMob") {
                                    HostileMob hostileMob = new HostileMob(float.Parse(entity.Attributes[0]), float.Parse(entity.Attributes[1]));
                                    hostileMob.MaxHealth = int.Parse(entity.Attributes[2]);
                                    hostileMob.Health = int.Parse(entity.Attributes[3]);
                                    hostileMob.DisplayHealth = hostileMob.Health;

                                    if (entity.Subnodes.Count > 0) {
                                        foreach (Node itemNode in entity.Subnodes) {
                                            if (itemNode.Name != "Item")
                                                continue;
                                            hostileMob.inventory.Items.Add(parseItem(itemNode.Attributes));
                                        }
                                    }

                                    map.Entities.Add(hostileMob);
                                }

                                if (entity.Name == "Corpse") {
                                    Corpse corpse = new Corpse(float.Parse(entity.Attributes[0]), float.Parse(entity.Attributes[1]));
                                    corpse.MaxHealth = int.Parse(entity.Attributes[2]);
                                    corpse.Health = int.Parse(entity.Attributes[3]);

                                    foreach (Node itemNode in entity.Subnodes) {
                                        if (itemNode.Name != "Item")
                                            continue;
                                        corpse.inventory.Items.Add(parseItem(itemNode.Attributes));
                                    }

                                    map.Entities.Add(corpse);
                                }

                                if (entity.Name == "GroundItem") {
                                    GroundItem groundItem = new GroundItem(parseItem(entity.Attributes.GetRange(2, entity.Attributes.Count - 2)), float.Parse(entity.Attributes[0]), float.Parse(entity.Attributes[1]));
                                    map.Items.Add(groundItem);
                                }
                            }
                        }
                    }
                    
                    gameState.Maps.Add(map);
                }
            }

            reader.Close();
        }
    }
}