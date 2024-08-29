using MySql.Data.MySqlClient;
using System;

namespace appLivros
{
    public class Program
    {
        private static string connectionString = "Server=localhost;Database=db_livro;Uid=root;Pwd=1234567889;SslMode=none;";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1 - Adicionar Livro");
                Console.WriteLine("2 - Listar Livros");
                Console.WriteLine("3 - Editar Livros");
                Console.WriteLine("4 - Excluir Livro");
                Console.WriteLine("5 - Sair");
                Console.Write("Escolha uma opção acima: ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        AdicionarLivro();
                        break;
                    case "2":
                        ListarLivro();
                        break;
                    case "3":
                        Editar();
                        break;
                    case "4":
                        Excluir();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Opção inválida");
                        break;
                }
            }
        }

        static void AdicionarLivro()
        {
            Console.Write("Informe o Titulo: ");
            string Titulo = Console.ReadLine();

            Console.Write("Informe o Autor: ");
            string Autor = Console.ReadLine();

            Console.Write("Informe o Ano de Publicação (yyyy-mm-dd): ");
            DateTime Data;
            while (!DateTime.TryParse(Console.ReadLine(), out Data))
            {
                Console.Write("Data inválida. Informe o Ano de Publicação (yyyy-mm-dd): ");
            }

            Console.Write("Informe o Genero: ");
            string Genero = Console.ReadLine();

            Console.Write("Informe o Numero de Paginas: ");
            int Pg;
            while (!int.TryParse(Console.ReadLine(), out Pg))
            {
                Console.Write("Número de páginas inválido. Informe o Numero de Paginas: ");
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO livro (Titulo, Autor, Data, Genero, Pg) VALUES (@Titulo, @Autor, @Data, @Genero, @Pg)";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@Titulo", Titulo);
                cmd.Parameters.AddWithValue("@Autor", Autor);
                cmd.Parameters.AddWithValue("@Data", Data);
                cmd.Parameters.AddWithValue("@Genero", Genero);
                cmd.Parameters.AddWithValue("@Pg", Pg);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Livro cadastrado com sucesso");
        }

        static void ListarLivro()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Titulo, Autor, Data, Genero, Pg FROM livro";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Titulo: {reader["Titulo"]}, Autor: {reader["Autor"]}, Data: {reader["Data"]}, Genero: {reader["Genero"]}, Pg: {reader["Pg"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Não existe livro cadastrado");
                    }
                }
            }
        }

        static void Excluir()
        {
            Console.Write("Informe o Id do Livro que deseja excluir: ");
            int idExclusao;
            while (!int.TryParse(Console.ReadLine(), out idExclusao))
            {
                Console.Write("ID inválido. Informe o Id do Livro que deseja excluir: ");
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM livro WHERE Id = @Id";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@Id", idExclusao);

                int linhaAfetada = cmd.ExecuteNonQuery();
                if (linhaAfetada > 0)
                {
                    Console.WriteLine("Livro excluído com sucesso");
                }
                else
                {
                    Console.WriteLine("Livro não encontrado");
                }
            }
        }

        static void Editar()
        {
            Console.Write("Informe o Id do Livro que deseja editar: ");
            int idEditar;
            while (!int.TryParse(Console.ReadLine(), out idEditar))
            {
                Console.Write("ID inválido. Informe o Id do Livro que deseja editar: ");
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM livro WHERE Id = @Id";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", idEditar);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.Write("Informe um novo Titulo: (* Deixe o campo em branco, para não alterar) ");
                        string novoTitulo = Console.ReadLine();

                        Console.Write("Informe um novo Autor: (* Deixe o campo em branco, para não alterar) ");
                        string novoAutor = Console.ReadLine();

                        Console.Write("Informe uma nova Data: (* Deixe o campo em branco, para não alterar) ");
                        string novaData = Console.ReadLine();

                        Console.Write("Informe um novo Genero: (* Deixe o campo em branco, para não alterar) ");
                        string novoGenero = Console.ReadLine();

                        Console.Write("Informe o novo Numero de Paginas: (* Deixe o campo em branco, para não alterar) ");
                        string novoPg = Console.ReadLine();

                        reader.Close();

                        string queryUpdate = "UPDATE livro SET Titulo = @Titulo, Autor = @Autor, Data = @Data, Genero = @Genero, Pg = @Pg WHERE Id = @Id";
                        cmd = new MySqlCommand(queryUpdate, connection);
                        cmd.Parameters.AddWithValue("@Titulo", string.IsNullOrWhiteSpace(novoTitulo) ? reader["Titulo"] : novoTitulo);
                        cmd.Parameters.AddWithValue("@Autor", string.IsNullOrWhiteSpace(novoAutor) ? reader["Autor"] : novoAutor);
                        cmd.Parameters.AddWithValue("@Data", string.IsNullOrWhiteSpace(novaData) ? reader["Data"] : DateTime.Parse(novaData));
                        cmd.Parameters.AddWithValue("@Genero", string.IsNullOrWhiteSpace(novoGenero) ? reader["Genero"] : novoGenero);
                        cmd.Parameters.AddWithValue("@Pg", string.IsNullOrWhiteSpace(novoPg) ? reader["Pg"] : int.Parse(novoPg));
                        cmd.Parameters.AddWithValue("@Id", idEditar);

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Livro atualizado com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("O Id do Livro não existe");
                    }
                }
            }
        }
    }
}
