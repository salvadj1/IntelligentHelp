using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Fougerite;

namespace IntelligentHelp
{
    public class IntelligentHelp : Fougerite.Module
    {
        public override string Name { get { return "IntelligentHelp"; } }
        public override string Author { get { return "Salva/juli"; } }
        public override string Description { get { return "IntelligentHelp"; } }
        public override Version Version { get { return new Version("1.0"); } }

        private string red = "[color #FF0000]";
        private string blue = "[color #81F7F3]";
        private string green = "[color #82FA58]";
        private string yellow = "[color #F4FA58]";
        private string orange = "[color #FF8000]";

        public List<string> listado = new List<string>();

        public override void Initialize()
        {
            Fougerite.Hooks.OnCommand += OnCommand;
            Fougerite.Hooks.OnServerInit += OnServerInit;
        }
        public override void DeInitialize()
        {
            Fougerite.Hooks.OnCommand -= OnCommand;
            Fougerite.Hooks.OnServerInit -= OnServerInit;
        }
        public void OnServerInit()
        {
            //Cargar listado de archivos al iniciar
            DirectoryInfo di = new DirectoryInfo(ModuleFolder);
            foreach (var x in di.GetFiles())
            {
                string nombrecompleto = x.Name;
                string[] split = nombrecompleto.Split(new[] { '.' });
                string nombre = split[0];
                string extension = split[1];
                listado.Add(nombre);
            }
        }
        public void OnCommand(Fougerite.Player player, string cmd, string[] args)
        {
            if (cmd == "help")
            {
                if (args.Length == 0)
                {
                    player.MessageFrom("Int.Help", "/help reload (ADMIN)");
                    foreach (var x in listado)
                    {
                        player.MessageFrom("Int.Help", "/help " + x);
                    }
                    return;
                }
                else
                {
                    //OPCION ADMIN
                    if (args[0] == "reload" && player.Admin)
                    {
                        listado.Clear();
                        player.MessageFrom("Int.Help", Name + " " + Version + " RELOADING!");
                        DirectoryInfo di = new DirectoryInfo(ModuleFolder);
                        foreach (var x in di.GetFiles())
                        {
                            string nombrecompleto = x.Name;
                            string[] split = nombrecompleto.Split(new[] { '.' });
                            string nombre = split[0];
                            string extension = split[1];
                            listado.Add(nombre);
                        }
                        player.MessageFrom("Int.Help", Name + " " + Version + " RELOADED!");
                        return;
                    }
                    //OPCION PLAYER
                    if (listado.Contains(args[0]))
                    {
                        if (File.Exists(Path.Combine(ModuleFolder, (args[0] + ".txt"))))
                        {
                            foreach (string str in File.ReadAllLines(Path.Combine(ModuleFolder, (args[0] + ".txt"))))
                            {
                                player.MessageFrom("Int.Help", str);
                            }
                        }
                        else
                        {
                            player.MessageFrom("Int.Help", "FILE NO FOUND :(");
                        }
                    }
                    else
                    {
                        player.MessageFrom("Int.Help", "This command no exits :(");
                    }
                }
            }
        }
    }
}
