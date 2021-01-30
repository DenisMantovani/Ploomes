using System;
using System.Globalization;
using System.Linq;
using ConsoleApp1.classes;
using Newtonsoft.Json.Linq;
using TestePloomes;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {


            Cliente cadastrandoCliente = new Cliente();
            Console.WriteLine("Insira o nome do cliente a ser adicionado:");
            cadastrandoCliente.Nome = (string)(Console.ReadLine());



            while ((cadastrandoCliente.TypeId != 1) && (cadastrandoCliente.TypeId != 2))
            {

                Console.WriteLine(" Digite 1 para pessoa fisica ou 2 para pessoa juridica:");
                cadastrandoCliente.TypeId = int.Parse(Console.ReadLine());

            }
            int validacao = 0;
            while (validacao <= 0)
            {
                try
                {

                    if (cadastrandoCliente.TypeId == 1)
                    {
                        try
                        {
                            Console.WriteLine(" Digite o numero do CPF:");
                            cadastrandoCliente.CPF = Console.ReadLine();//criar mascara
                            validacao += 1;
                        }
                        catch
                        {

                        }
                    }
                    if (cadastrandoCliente.TypeId == 2)
                    {
                        try
                        {
                            Console.WriteLine(" Digite o numero do Cnpj:");
                            cadastrandoCliente.CNPJ = Console.ReadLine();//criar mascara
                            validacao += 1;
                        }
                        catch
                        {

                        }
                    }
                }


                catch (FormatException e)
                {
                    Console.WriteLine(" Use apenas os numeros ");
                }
            }
            JObject novoCliente = new JObject();
            novoCliente.Add("Name", cadastrandoCliente.Nome);
            novoCliente.Add("TypeId", cadastrandoCliente.TypeId);
            novoCliente.Add("CPF", cadastrandoCliente.CPF);
            novoCliente.Add("CNPJ", cadastrandoCliente.CNPJ);

            JArray ClienteCadastro = RequestHandler.MakePloomesRequest($"Contacts", RestSharp.Method.POST, novoCliente);
            if (ClienteCadastro.Count > 0)
            {
                int idCriado = (int)ClienteCadastro[0]["Id"];
                cadastrandoCliente.Id = idCriado;
                Console.WriteLine(" Cliente cadastrado ");
            }
            else
            {
                Console.WriteLine(" Cliente não cadastrado tente novamente ");
            }

            /////////////////////////

            Negociacao cadastrandoNegociacao = new Negociacao();

            Console.WriteLine("Digite um titulo para negociação:");
            cadastrandoNegociacao.Titulo = (string)(Console.ReadLine());


            JObject negociacao = new JObject();
            negociacao.Add("Title", cadastrandoNegociacao.Titulo);
            negociacao.Add("ContactId", cadastrandoCliente.Id);

            JArray NegociacaoCadastro = RequestHandler.MakePloomesRequest($"Deals", RestSharp.Method.POST, negociacao);

            if (NegociacaoCadastro.Count > 0)
            {
                int idNegociacao = (int)NegociacaoCadastro[0]["Id"];
                cadastrandoNegociacao.Id = idNegociacao;
            }
            ///////////////////////
            Tarefa cadastrandoTarefa = new Tarefa();

            Console.WriteLine("Digite um titulo para tarefa:");
            cadastrandoTarefa.Titulo = (string)(Console.ReadLine());


            Console.WriteLine("Digite o dia para o fim da tarefa");
            int dia = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Digite o mes para o fim da tarefa ");
            int mes = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Digite o ano para o fim da tarefa ");
            int ano = Convert.ToInt32(Console.ReadLine());
            DateTime dataTermino = new DateTime(ano, mes, dia);
            


            //Console.WriteLine("Utilize apenas numeros");



            JObject tarefa = new JObject();
            tarefa.Add("Title", cadastrandoTarefa.Titulo);
            tarefa.Add("ContactId", cadastrandoCliente.Id);
            //tarefa.Add("DateTime", dataTermino);
            JArray TarefaCadastro = RequestHandler.MakePloomesRequest($"Tasks", RestSharp.Method.POST, tarefa);
            if (TarefaCadastro.Count > 0)
            {
                int idTarefa = (int)TarefaCadastro[0]["Id"];
                cadastrandoTarefa.Id = idTarefa;
            }
            Console.WriteLine("cadastrar tarefa ok");
  
            JObject atualizarValor = new JObject
            {
                {"Amount", "15000"}
            };
            RequestHandler.MakePloomesRequest($"Deals({cadastrandoNegociacao.Id})", RestSharp.Method.PATCH, atualizarValor);
            Console.WriteLine("amount ok");
            ///////

            JObject finalizarTarefa = new JObject
            {
                {"Finished", "true" }
            };
            RequestHandler.MakePloomesRequest($"Tasks({cadastrandoTarefa.Id})/Finish", RestSharp.Method.POST, finalizarTarefa);
            Console.WriteLine("finalizar ok");
            //////////
            JObject ganharNegociacao = new JObject
            {
                {"StatusId", "2" }
            };
            RequestHandler.MakePloomesRequest($"Deals({cadastrandoNegociacao.Id})/Win", RestSharp.Method.POST, ganharNegociacao);
            Console.WriteLine("win ok");
            //////////
            JObject fechandoNegocio = new JObject();
            string fechado = "Negocio fechado";
            fechandoNegocio.Add("Content", fechado);
            fechandoNegocio.Add("Id", cadastrandoCliente.Id);

            RequestHandler.MakePloomesRequest($"InteractionRecord", RestSharp.Method.POST, fechandoNegocio);
            Console.WriteLine("mensagem  ok");






        }
    }

}
 


