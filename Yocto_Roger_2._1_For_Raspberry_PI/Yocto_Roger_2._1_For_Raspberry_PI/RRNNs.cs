using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Device.Gpio; // Download it from nuget.org (https://www.nuget.org/packages/System.Device.Gpio/4.1.0?_src=template)
using System.Device.I2c; // its downloading together with gpio
using Yocto_Roger_2._1_For_Raspberry_PI;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace Yocto_Roger_2._1_For_Raspberry_PI
{

    /*
     Это  RRNNs!
     RRNNs - Robot Realtime Neural Network System (Быстродейственная система нейронных сетей для роботов)
    ***********
    Написано семечкой из команды "Emotion Corp ;)" В помощь Аксолотлю
    ***********
    
    Это своего рода API (По сути он и есть) для взаимодействия с нейросетью, с помощью роботов (ну или же для плат на подобии Raspberry PI)
    Надеюсь код более менее понятен...

    Как всегда аксолотль, семечка, хамелеон, грибочек.
    По кирпичикам построим мечту!
     */
    internal class RRNNs
    {
        string date_time = System.DateTime.Now.ToString(); // will be used in the future (I hope i dont forget...)
        static public Dictionary<string, int> pins = new();
        // Почему закомментированно, читать в конце файла
        // public static GpioController controller = new(); // Raspberry PI
        public static bool is_need_date_time = false;

        /*
         Dictionary keys:
            for_time // local time. Example: 26.02.2026 21:04:10
            input_neurons
            output_neurons
         */

        static public void setUpPins()
        {
            int i = 0;

            LoadPinsSettings(); // If file dont exists, function do nothing
            while (true)
            {
                Console.Write(
                    "1 - Save and back to main menu\n" +
                    "2. Pin for displaying date and time (output mode, default: off)\n" +
                    "3. Pin for input neurons (input mode)\n" +
                    "4. Pin for output neurons (output mode)\n" +
                    ">>> ");

                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    bool exit = false;

                    string[] answers = new string[3]
                    {
                    "Please, enter the pin number: ",
                    "Please, enter the pin number again: ",
                    "Sorry, but please, enter the pin number again, i hope, its one last time: "
                    };
                    switch (input)
                    {
                        case 1:
                            Console.Clear();
                            exit = true;
                            savePinsSettins();
                            break;

                        case 2:
                            Console.Write(answers[i]);
                            pins["for_time"] = Convert.ToInt32(Console.ReadLine());
                            is_need_date_time = true;
                            i++;
                            break;

                        case 3:
                            Console.Write(answers[i]);
                            pins["input_neurons"] =  Convert.ToInt32(Console.ReadLine());
                            i++;
                            break;

                        case 4:
                            Console.Write(answers[i]);
                            pins["output_neurons"] = Convert.ToInt32(Console.ReadLine());
                            i++;
                            break;

                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Incorrect input >:(");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                    }

                    if (exit == true)
                        break;

                    if (i > 2)
                        i = 0;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect input >:(");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

        static private void savePinsSettins()
        {
            try
            {
                if (File.Exists(Parameters.pins_settings))
                {
                    Console.WriteLine("The settings file exists, I'm overwriting it.");
                }
                else
                {
                    using(System.IO.FileStream fs = File.Create(Parameters.pins_settings))
                    {
                    }
                    Console.WriteLine("File is maked! Writing data...");
                }

                var options = new JsonSerializerOptions { WriteIndented = true }; // Makes json readable

                if (is_need_date_time == true)
                {
                    var preJsonData = new
                    {
                        pin_for_time = pins["for_time"],
                        pin_for_input_neurons = pins["input_neurons"],
                        pin_for_output_neurons = pins["output_neurons"]
                    };
                    var json = JsonSerializer.Serialize(preJsonData, options);
                    File.WriteAllText(Parameters.pins_settings, json);
                    Console.WriteLine("Yappyyyy, i'm write data sucessfull!");
                }
                else if (is_need_date_time == false)
                {
                    var preJsonData = new
                    {
                        pin_for_input_neurons = pins["input_neurons"],
                        pin_for_output_neurons = pins["output_neurons"]
                    };
                    var json = JsonSerializer.Serialize(preJsonData, options);

                    File.WriteAllText(Parameters.pins_settings, json);

                    Console.WriteLine("Yappyyyy, i'm write data sucessfull!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Sorry, maybe i can't make a file for my pins settings (or something else...), error log: \n{e}");
                Thread.Sleep(1000); // To have time to read log
            }
        }

        static private void LoadPinsSettings()
        {
            if (File.Exists(Parameters.pins_settings))
            {
                string json_settings = File.ReadAllText(Parameters.pins_settings);

                using (JsonDocument document = JsonDocument.Parse(json_settings)) 
                {
                    JsonElement elements = document.RootElement; 
                    if (is_need_date_time == false)
                    {
                        pins["input_neurons"] = elements.GetProperty("pin_for_input_neurons").GetInt32();
                        pins["output_neurons"] = elements.GetProperty("pin_for_output_neurons").GetInt32();
                    }
                    else if (is_need_date_time == true)
                    {
                        pins["for_time"] = elements.GetProperty("pin_for_time").GetInt32();
                        pins["input_neurons"] = elements.GetProperty("pin_for_input_neurons").GetInt32();
                        pins["output_neurons"] = elements.GetProperty("pin_for_output_neurons").GetInt32();
                    }
                }
            }
        }

        // Я закомментировал всё что связано в GPIO чтобы не выходило ошибки что платформы не совместимы, чтобы можно было на винде тестировать. Доделанную версию залью чуть позже (на следующий день буду писать)
        //static public void TalkWithRaspberry()
        //{
        //    if (is_need_date_time == true)
        //        controller.OpenPin(pins["for_time"], PinMode.Output);

        //    controller.OpenPin(pins["input_neurons"], PinMode.Input);
        //    controller.OpenPin(pins["output_neurons"], PinMode.Output);
        //}

    }
}
