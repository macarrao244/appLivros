using MySql.Data.MySqlClient;
using System;

namespace appEscola
{
    public class Program
    {
        private static string connectionString = "Server=localhost;Database=db_alunos;Uid=root;Pwd=1234567889;SslMode=none;";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1 - Adicionar Aluno");
                Console.WriteLine("2 - Listar Todos os Alunos");
                Console.WriteLine("3 - Editar Aluno");
                Console.WriteLine("4 - Excluir Aluno");
                Console.WriteLine("5 - Listar Alunos por Curso");
                Console.WriteLine("6 - Sair");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();

                try
                {
                    switch (opcao)
                    {
                        case "1":
                            AdicionarAluno();
                            break;
                        case "2":
                            ListarAlunos();
                            break;
                        case "3":
                            EditarAluno();
                            break;
                        case "4":
                            ExcluirAluno();
                            break;
                        case "5":
                            ListarAlunosPorCurso();
                            break;
                        case "6":
                            return;
                        default:
                            Console.WriteLine("Opção inválida");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro:");
                }
            }
        }

        static void AdicionarAluno()
        {
            Console.Write("Informe o Nome: ");
            string nome = Console.ReadLine();

            Console.Write("Informe a Idade: ");
            int idade;
            while (!int.TryParse(Console.ReadLine(), out idade))
            {
                Console.Write("Idade inválida. Informe a Idade: ");
            }

            Console.Write("Informe o Curso: ");
            string curso = Console.ReadLine();

            Console.Write("Informe a Data de Matrícula : ");
            DateTime DataMatricula;
            while (!DateTime.TryParse(Console.ReadLine(), out DataMatricula))
            {
                Console.Write("Data inválida. Informe a Data de Matrícula : ");
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "INSERT INTO alunos (Nome, Idade, Curso, DataMatricula) VALUES (@Nome, @Idade, @Curso, @DataMatricula)";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Nome", nome);
                        cmd.Parameters.AddWithValue("@Idade", idade);
                        cmd.Parameters.AddWithValue("@Curso", curso);
                        cmd.Parameters.AddWithValue("@DataMatricula", DataMatricula);
                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Aluno cadastrado com sucesso");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar aluno: ");
            }
        }

        static void ListarAlunos()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT Id, Nome, Idade, Curso, DataMatricula FROM alunos";
                    using (var cmd = new MySqlCommand(query, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"Id: {reader["Id"]}, Nome: {reader["Nome"]}, Idade: {reader["Idade"]}, Curso: {reader["Curso"]}, Data de Matrícula: {reader["DataMatricula"]}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Não há alunos cadastrados");
                        }
                    }
                }
            }
            catch 
            {
                Console.WriteLine($"Não foi possivel listar alunos:");
            }
        }

        static void EditarAluno()
        {
            Console.Write("Informe o Id do Aluno que deseja editar: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.Write("ID inválido. Informe o Id do Aluno que deseja editar: ");
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM alunos WHERE Id = @Id";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                reader.Close();

                                Console.Write("Informe um novo Nome: (* Deixe o campo em branco, para não alterar) ");
                                string novoNome = Console.ReadLine();

                                Console.Write("Informe uma nova Idade: (* Deixe o campo em branco, para não alterar) ");
                                string novaIdade = Console.ReadLine();

                                Console.Write("Informe um novo Curso: (* Deixe o campo em branco, para não alterar) ");
                                string novoCurso = Console.ReadLine();

                                Console.Write("Informe uma nova Data de Matrícula :  (* Deixe o campo em branco, para não alterar) ");
                                string novaDataMatricula = Console.ReadLine();

                                var queryUpdate = "UPDATE alunos SET Nome = @Nome, Idade = @Idade, Curso = @Curso, DataMatricula = @DataMatricula WHERE Id = @Id";
                                using (var cmdUpdate = new MySqlCommand(queryUpdate, connection))
                                {
                                    cmdUpdate.Parameters.AddWithValue("@Nome", string.IsNullOrWhiteSpace(novoNome) ? reader["Nome"] : novoNome);
                                    cmdUpdate.Parameters.AddWithValue("@Idade", string.IsNullOrWhiteSpace(novaIdade) ? reader["Idade"] : int.Parse(novaIdade));
                                    cmdUpdate.Parameters.AddWithValue("@Curso", string.IsNullOrWhiteSpace(novoCurso) ? reader["Curso"] : novoCurso);
                                    cmdUpdate.Parameters.AddWithValue("@DataMatricula", string.IsNullOrWhiteSpace(novaDataMatricula) ? reader["Data"] : DateTime.Parse(novaDataMatricula)); cmdUpdate.Parameters.AddWithValue("@Id", id);
                                    cmdUpdate.ExecuteNonQuery();
                                    Console.WriteLine("Aluno atualizado com sucesso!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("O Id do Aluno não existe");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao editar aluno: {ex.Message}");
            }
        }

        static void ExcluirAluno()
        {
            Console.Write("Informe o Id do Aluno que deseja excluir: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.Write("ID inválido. Informe o Id do Aluno que deseja excluir: ");
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "DELETE FROM alunos WHERE Id = @Id";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        int linhaAfetada = cmd.ExecuteNonQuery();
                        if (linhaAfetada > 0)
                        {
                            Console.WriteLine("Aluno excluído com sucesso");
                        }
                        else
                        {
                            Console.WriteLine("Aluno não encontrado");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir aluno: {ex.Message}");
            }
        }

        static void ListarAlunosPorCurso()
        {
            Console.Write("Informe o Curso para listar alunos: ");
            string curso = Console.ReadLine();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT Id, Nome, Idade, Curso, DataMatricula FROM alunos WHERE Curso = @Curso";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Curso", curso);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine($"Id: {reader["Id"]}, Nome: {reader["Nome"]}, Idade: {reader["Idade"]}, Curso: {reader["Curso"]}, Data de Matrícula: {reader["DataMatricula"]}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Não há alunos matriculados nesse curso");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao listar alunos por curso: {ex.Message}");
            }
        }
    }
}
