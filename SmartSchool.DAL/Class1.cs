using SmartSchool.DTOs;
using System.Data;
using Microsoft.Data.SqlClient;
namespace SmartSchool.DAL
{
    public static class UserDal
    {
        public static OperationResult<UserDTO> GetUserByEmail(string Email, string connectionString)
        {
            var result = new OperationResult<UserDTO>();
            result.Data = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetUserByEmail", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", Email);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Data = new UserDTO
                            {
                                Username = reader["FullName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Role = reader["Role"].ToString(),
                                UserId = (int)reader["UserId"],
                                IsActive = (bool)reader["IsActive"],
                                CreatedAt = (DateTime)reader["CreatedAt"]
                            };
                            result.Success = true;
                            result.Message = "User found successfully.";
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = "User not found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }
            return result;
        }
        public static int CreateUser(CreateUserDTO user, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_CreateUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FullName", user.Username);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.Password);
                    cmd.Parameters.AddWithValue("@Role", user.Role);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int UpdateUser(UpdateUserDTO user, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdateUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", user.UserId);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Role", user.Role);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int DeleteUser(int userId, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static OperationResult<UserDTO> GetUserById(int userId, string connectionString)
        {
            var result = new OperationResult<UserDTO>();
            result.Data = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetUserById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Data = new UserDTO
                            {
                                UserId = (int)reader["UserId"],
                                Username = reader["FullName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Role = reader["Role"].ToString(),
                                IsActive = (bool)reader["IsActive"],
                                CreatedAt = (DateTime)reader["CreatedAt"]
                            };
                            result.Success = true;
                            result.Message = "Teacher found successfully.";
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = "Teacher not found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }
            return result;
        }
        public static OperationResult<List<UserDTO>> GetAllUsers(string connectionString)
        {
            var result = new OperationResult<List<UserDTO>>()
            {
                Data = new List<UserDTO>()
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllActiveUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Data.Add(new UserDTO
                            {
                                UserId = (int)reader["UserId"],
                                Username = reader["FullName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Role = reader["Role"].ToString(),
                                IsActive = (bool)reader["IsActive"],
                                CreatedAt = (DateTime)reader["CreatedAt"]
                            });
                        }
                    }
                }

                if (result.Data.Count > 0)
                {
                    result.Success = true;
                    result.Message = "Teachers retrieved successfully.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "No teachers found.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
    }
    public class TeacherDal
    {
        public static int CreateTeacher(CreateTeacherDTO teacher, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_CreateTeacher", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FullName", teacher.FullName);
                    cmd.Parameters.AddWithValue("@Email", teacher.Email);
                    cmd.Parameters.AddWithValue("@Password", teacher.Password);
                    cmd.Parameters.AddWithValue("@Specialization", teacher.Specialization);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int UpdateTeacher(UpdateTeacherDTO teacher, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdateTeacher", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TeacherId", teacher.TeacherId);
                    cmd.Parameters.AddWithValue("@FullName", teacher.FullName);
                    cmd.Parameters.AddWithValue("@Email", teacher.Email);
                    cmd.Parameters.AddWithValue("@HireDate", teacher.HireDate);
                    cmd.Parameters.AddWithValue("@Specialization", teacher.Specialization);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int DeleteTeacher(int teacherId, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteTeacher", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TeacherId", teacherId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static OperationResult<TeacherDTO> GetTeacherById(int teacherId, string connectionString)
        {
            var result = new OperationResult<TeacherDTO>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetTeacherById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TeacherId", teacherId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Data = new TeacherDTO
                            {
                                TeacherId = (int)reader["TeacherId"],
                                FullName = reader["FullName"].ToString(),
                                Email = reader["Email"].ToString(),
                                HireDate = (DateTime)reader["HireDate"],
                                Specialization = reader["Specialization"].ToString(),
                                Status = reader["Status"].ToString()
                            };
                            result.Success = true;
                            result.Message = "Teacher found successfully.";
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = "Teacher not found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }
            return result;
        }
        public static OperationResult<List<TeacherDTO>> GetAllActiveTeachers(string connectionString)
        {
            var result = new OperationResult<List<TeacherDTO>>()
            {
                Data = new List<TeacherDTO>()
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllActiveTeachers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Data.Add(new TeacherDTO
                            {
                                TeacherId = (int)reader["TeacherId"],
                                FullName = reader["FullName"].ToString(),
                                Email = reader["Email"].ToString(),
                                HireDate = (DateTime)reader["HireDate"],
                                Specialization = reader["Specialization"].ToString(),
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }

                if (result.Data.Count > 0)
                {
                    result.Success = true;
                    result.Message = "Teachers retrieved successfully.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "No teachers found.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
    }
    public class StudentDal
    {
        public static int UpdateStudent(UpdateStudentDTO student, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))

                using (SqlCommand cmd = new SqlCommand("sp_UpdateStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", student.StudentId);
                    cmd.Parameters.AddWithValue("@FullName", student.FullName);
                    cmd.Parameters.AddWithValue("@Email", student.Email);
                    cmd.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", student.Gender);
                    cmd.Parameters.AddWithValue("@GuardianName", student.GuardianName);
                    cmd.Parameters.AddWithValue("@GuardianPhone", student.GuardianPhone);
                    cmd.Parameters.AddWithValue("@SectionId", student.SectionId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int DeleteStudent(int studentId, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int CreateStudent(CreateStudentDTO student, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CreateStudent", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@FullName", student.FullName);
                        cmd.Parameters.AddWithValue("@Email", student.Email);
                        cmd.Parameters.AddWithValue("@Password", student.Password);
                        cmd.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                        cmd.Parameters.AddWithValue("@Gender", student.Gender);
                        cmd.Parameters.AddWithValue("@GuardianName", student.GuardianName);
                        cmd.Parameters.AddWithValue("@GuardianPhone", student.GuardianPhone);
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception)
            {
                return -99;
            }
        }
        public static OperationResult<StudentDTO> GetStudentById(int studentId, string connectionString)
        {
            var result = new OperationResult<StudentDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetStudentById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Data = new StudentDTO
                            {
                                StudentId = (int)reader["StudentId"],
                                FullName = reader["FullName"].ToString(),
                                AdmissionNo = reader["AdmissionNo"].ToString(),
                                DateOfBirth = (DateTime)reader["DateOfBirth"],
                                Gender = reader["Gender"].ToString(),
                                GuardianName = reader["GuardianName"].ToString(),
                                GuardianPhone = reader["GuardianPhone"].ToString(),
                                ClassName = reader["ClassName"].ToString(),
                                SectionName = reader["SectionName"].ToString(),
                                Status = reader["Status"].ToString()
                            };
                            result.Success = true;
                            result.Message = "Student found successfully.";
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = "Student not found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static OperationResult<List<StudentDTO>> GetAllActiveStudents(string connectionString)
        {
            var result = new OperationResult<List<StudentDTO>>()
            {
                Data = new List<StudentDTO>()
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllActiveStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Data.Add(new StudentDTO
                            {
                                StudentId = (int)reader["StudentId"],
                                FullName = reader["FullName"].ToString(),
                                AdmissionNo = reader["AdmissionNo"].ToString(),
                                DateOfBirth = (DateTime)reader["DateOfBirth"],
                                Gender = reader["Gender"].ToString(),
                                GuardianName = reader["GuardianName"].ToString(),
                                GuardianPhone = reader["GuardianPhone"].ToString(),
                                ClassName = reader["ClassName"].ToString(),
                                SectionName = reader["SectionName"].ToString(),
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }

                if (result.Data.Count > 0)
                {
                    result.Success = true;
                    result.Message = "Students retrieved successfully.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "No students found.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }
            return result;
        }
    }
    public class ClassDal
    {
        public static OperationResult<List<ClassDTO>> GetAllClasses(string connectionString)
        {
            var result = new OperationResult<List<ClassDTO>>()
            {
                Data = new List<ClassDTO>()
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllClasses", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Data.Add(new ClassDTO
                            {
                                ClassId = (int)reader["ClassId"],
                                ClassName = reader["ClassName"].ToString(),
                                Level = (int)reader["GradeLevel"]
                            });
                        }
                    }
                }

                result.Success = result.Data.Count > 0;
                result.Message = result.Success ? "Classes retrieved successfully." : "No classes found.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
    }
    public class SectionDal
    {
        public static int CreateSection(CreateSectionDTO section, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_CreateSection", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClassId", section.ClassId);
                    cmd.Parameters.AddWithValue("@SectionName", section.SectionName);
                    cmd.Parameters.AddWithValue("@Capacity", section.Capacity);
                    cmd.Parameters.AddWithValue("@HomeroomTeacherId", section.HomeroomTeacherId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int UpdateSection(UpdateSectionDTO section, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdateSection", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SectionId", section.SectionId);
                    cmd.Parameters.AddWithValue("@ClassId", section.ClassId);
                    cmd.Parameters.AddWithValue("@SectionName", section.SectionName);
                    cmd.Parameters.AddWithValue("@Capacity", section.Capacity);
                    cmd.Parameters.AddWithValue("@HomeroomTeacherId", section.HomeroomTeacherId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int DeleteSection(int sectionId, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteSection", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SectionId", sectionId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static OperationResult<SectionDTO> GetSectionById(int sectionId, string connectionString)
        {
            var result = new OperationResult<SectionDTO>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetSectionById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SectionId", sectionId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Data = new SectionDTO
                            {
                                SectionId = (int)reader["SectionId"],
                                SectionName = reader["SectionName"].ToString(),
                                Capacity = (int)reader["Capacity"],
                                ClassName = reader["ClassName"].ToString(),
                                TeacherId = (int)reader["TeacherId"],
                                TeacherName = reader["TeacherName"].ToString(),
                                Status = reader["Status"].ToString()
                            };
                            result.Success = true;
                            result.Message = "Section found successfully.";
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = "Section not found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }
            return result;
        }
        public static OperationResult<List<SectionDTO>> GetAllActiveSections(string connectionString)
        {
            var result = new OperationResult<List<SectionDTO>>()
            {
                Data = new List<SectionDTO>()
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllActiveSections", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Data.Add(new SectionDTO
                            {
                                SectionId = (int)reader["SectionId"],
                                SectionName = reader["SectionName"].ToString(),
                                Capacity = (int)reader["Capacity"],
                                ClassName = reader["ClassName"].ToString(),
                                TeacherId = (int)reader["TeacherId"],
                                TeacherName = reader["TeacherName"].ToString(),
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }

                if (result.Data.Count > 0)
                {
                    result.Success = true;
                    result.Message = "Sections retrieved successfully.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "No sections found.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }
            return result;
        }
    }
    public class SubjectDal
    {
        public static OperationResult<SubjectDTO> GetSubjectById(int subjectId, string connectionString)
        {
            var result = new OperationResult<SubjectDTO>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetSubjectById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SubjectId", subjectId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Data = new SubjectDTO
                            {
                                SubjectId = (int)reader["SubjectId"],
                                SubjectName = reader["SubjectName"].ToString(),
                                SubjectCode = reader["SubjectCode"].ToString(),
                                CreditHours = (int)reader["CreditHours"],
                                Status = reader["Status"].ToString()
                            };
                            result.Success = true;
                            result.Message = "Subject found successfully.";
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = "Subject not found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }
            return result;
        }
        public static int CreateSubject(CreateSubjectDTO subject, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_CreateSubject", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SubjectName", subject.SubjectName);
                    cmd.Parameters.AddWithValue("@SubjectCode", subject.SubjectCode);
                    cmd.Parameters.AddWithValue("@CreditHours", subject.CreditHours);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int UpdateSubject(UpdateSubjectDTO subject, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdateSubject", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SubjectId", subject.SubjectId);
                    cmd.Parameters.AddWithValue("@SubjectName", subject.SubjectName);
                    cmd.Parameters.AddWithValue("@SubjectCode", subject.SubjectCode);
                    cmd.Parameters.AddWithValue("@CreditHours", subject.CreditHours);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int DeleteSubject(int subjectId, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteSubject", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SubjectId", subjectId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static OperationResult<List<SubjectDTO>> GetAllActiveSubjects(string connectionString)
        {
            var result = new OperationResult<List<SubjectDTO>> { Data = new List<SubjectDTO>() };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllActiveSubjects", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Data.Add(new SubjectDTO
                            {
                                SubjectId = (int)reader["SubjectId"],
                                SubjectName = reader["SubjectName"].ToString(),
                                SubjectCode = reader["SubjectCode"].ToString(),
                                CreditHours = (int)reader["CreditHours"],
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }

                result.Success = result.Data.Count > 0;
                result.Message = result.Success ? "Subjects retrieved successfully." : "No subjects found.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
    }
    public class ClassSubjectDal
    {
        public static int CreateClassSubject(CreateClassSubjectDTO dto, string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_CreateClassSubject", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClassId", dto.ClassId);
                cmd.Parameters.AddWithValue("@SubjectId", dto.SubjectId);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            catch
            {
                return -99;
            }
        }
        public static OperationResult<List<ClassSubjectDTO>> GetClassSubjectsByClass(int classId, string connectionString)
        {
            var result = new OperationResult<List<ClassSubjectDTO>> { Data = new List<ClassSubjectDTO>() };

            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_GetClassSubjectsByClass", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassId", classId);

                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Data.Add(new ClassSubjectDTO
                    {
                        ClassSubjectId = (int)reader["ClassSubjectId"],
                        ClassName = reader["ClassName"].ToString(),
                        SubjectId = (int)reader["SubjectId"],
                        SubjectName = reader["SubjectName"].ToString(),
                        Code = reader["Code"].ToString(),
                        CreditHours = (int)reader["CreditHours"]
                    });
                }

                result.Success = result.Data.Count > 0;
                result.Message = result.Success ? "Subjects retrieved successfully." : "No subjects found.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static int DeleteClassSubject(int classSubjectId, string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_DeleteClassSubject", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassSubjectId", classSubjectId);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            catch
            {
                return -99;
            }
        }
    }
    public class EnrollmentDal
    {
        public static int UpdateEnrollment(UpdateEnrollmentDTO enrollment, string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("sp_UpdateEnrollment", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EnrollmentId", enrollment.SectionId);
                cmd.Parameters.AddWithValue("@NewSectionId", enrollment.EnrollmentId);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                return -99;
            }
        }
        public static int CreateEnrollment(CreateEnrollmentDTO enrollment, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_CreateEnrollment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", enrollment.StudentId);
                    cmd.Parameters.AddWithValue("@SectionId", enrollment.SectionId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int DeleteEnrollment(int enrollmentId, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteEnrollment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EnrollmentId", enrollmentId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static OperationResult<List<EnrollmentDTO>> GetAllEnrollments(string connectionString)
        {
            var result = new OperationResult<List<EnrollmentDTO>>() { Data = new List<EnrollmentDTO>() };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllEnrollments", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Data.Add(new EnrollmentDTO
                            {
                                EnrollmentId = (int)reader["EnrollmentId"],
                                StudentId = (int)reader["StudentId"],
                                StudentName = reader["StudentName"].ToString(),
                                SectionId = (int)reader["SectionId"],
                                SectionName = reader["SectionName"].ToString(),
                                ClassName = reader["ClassName"].ToString(),
                                EnrollmentDate = (DateTime)reader["EnrollmentDate"],
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }

                result.Success = result.Data.Count > 0;
                result.Message = result.Success ? "Enrollments retrieved successfully." : "No enrollments found.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static OperationResult<List<EnrollmentDTO>> GetEnrollmentsByStudent(int studentId, string connectionString)
        {
            var result = new OperationResult<List<EnrollmentDTO>>() { Data = new List<EnrollmentDTO>() };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetEnrollmentsByStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows && reader.FieldCount == 1 && reader.GetName(0) == "Result")
                        {
                            result.Success = false;
                            result.Message = "Student not found.";
                            return result;
                        }

                        while (reader.Read())
                        {
                            result.Data.Add(new EnrollmentDTO
                            {
                                EnrollmentId = (int)reader["EnrollmentId"],
                                SectionName = reader["SectionName"].ToString(),
                                ClassName = reader["ClassName"].ToString(),
                                StudentName = reader["StudentName"].ToString(),
                                EnrollmentDate = (DateTime)reader["EnrollmentDate"],
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }

                result.Success = result.Data.Count > 0;
                result.Message = result.Success ? "Student enrollments retrieved successfully." : "No enrollments found for this student.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static OperationResult<List<EnrollmentDTO>> GetStudentsBySection(int sectionId, string connectionString)
        {
            var result = new OperationResult<List<EnrollmentDTO>>() { Data = new List<EnrollmentDTO>() };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetStudentsBySection", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SectionId", sectionId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows && reader.FieldCount == 1 && reader.GetName(0) == "Result")
                        {
                            result.Success = false;
                            result.Message = "Section not found.";
                            return result;
                        }
                        while (reader.Read())
                        {
                            result.Data.Add(new EnrollmentDTO
                            {
                                EnrollmentId = (int)reader["EnrollmentId"],
                                StudentId = (int)reader["StudentId"],
                                StudentName = reader["StudentName"].ToString(),
                                EnrollmentDate = (DateTime)reader["EnrollmentDate"],
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }

                result.Success = result.Data.Count > 0;
                result.Message = result.Success ? "Students retrieved successfully." : "No students found in this section.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
    }
    public class ExamDal
    {
        public static int CreateExam(CreateExamDTO exam, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_CreateExam", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", exam.Name);
                    cmd.Parameters.AddWithValue("@StartDate", exam.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", exam.EndDate);
                    cmd.Parameters.AddWithValue("@Type", exam.Type);
                    cmd.Parameters.AddWithValue("@AcademicYear", exam.AcademicYear);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int UpdateExam(UpdateExamDTO exam, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdateExam", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExamId", exam.ExamId);
                    cmd.Parameters.AddWithValue("@Name", exam.Name);
                    cmd.Parameters.AddWithValue("@StartDate", exam.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", exam.EndDate);
                    cmd.Parameters.AddWithValue("@Type", exam.Type);
                    cmd.Parameters.AddWithValue("@AcademicYear", exam.AcademicYear);


                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int DeleteExam(int examId, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteExam", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExamId", examId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static OperationResult<List<ExamDTO>> GetAllActiveExams(string connectionString)
        {
            var result = new OperationResult<List<ExamDTO>> { Data = new List<ExamDTO>() };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllActiveExams", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Data.Add(new ExamDTO
                            {
                                ExamId = (int)reader["ExamId"],
                                Name = reader["Name"].ToString(),
                                StartDate = (DateTime)reader["StartDate"],
                                EndDate = (DateTime)reader["EndDate"],
                                Type = reader["Type"].ToString(),
                                AcademicYear = (int)reader["AcademicYear"],
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }

                result.Success = result.Data.Count > 0;
                result.Message = result.Success ? "Exams retrieved successfully." : "No exams found.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
    }
    public class ExamResultDal
    {
        public static int CreateExamResult(CreateExamResultDTO result, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_CreateExamResult", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExamId", result.ExamId);
                    cmd.Parameters.AddWithValue("@EnrollmentId", result.EnrollmentId);
                    cmd.Parameters.AddWithValue("@SubjectId", result.SubjectId);
                    cmd.Parameters.AddWithValue("@MarksObtained", result.MarksObtained);
                    cmd.Parameters.AddWithValue("@Remarks", result.Remarks ?? (object)DBNull.Value);

                    conn.Open();
                    object dbResult = cmd.ExecuteScalar();
                    return Convert.ToInt32(dbResult);
                }
            }
            catch { return -99; }
        }
        public static int UpdateExamResult(UpdateExamResultDTO result, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdateExamResult", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ResultId", result.ResultId);
                    cmd.Parameters.AddWithValue("@MarksObtained", result.MarksObtained);
                    cmd.Parameters.AddWithValue("@Remarks", result.Remarks ?? (object)DBNull.Value);

                    conn.Open();
                    object dbResult = cmd.ExecuteScalar();
                    return Convert.ToInt32(dbResult);
                }
            }
            catch { return -99; }
        }
        public static int DeleteExamResult(int resultId, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteExamResult", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ResultId", resultId);

                    conn.Open();
                    object dbResult = cmd.ExecuteScalar();
                    return Convert.ToInt32(dbResult);
                }
            }
            catch { return -99; }
        }
        public static OperationResult<List<ExamResultDTO>> GetExamResultsByExam(int examId, string connectionString)
        {
            var result = new OperationResult<List<ExamResultDTO>> { Data = new List<ExamResultDTO>() };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetExamResultsByExam", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExamId", examId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Data.Add(new ExamResultDTO
                            {
                                ResultId = (int)reader["ResultId"],
                                ExamId = examId,
                                ExamName = reader["ExamName"].ToString(),
                                ExamType = reader["ExamType"].ToString(),
                                EnrollmentId = (int)reader["EnrollmentId"],
                                StudentName = reader["StudentName"].ToString(),
                                SubjectId = (int)reader["SubjectId"],
                                SubjectName = reader["SubjectName"].ToString(),
                                MarksObtained = (decimal)reader["MarksObtained"],
                                Grade = reader["Grade"].ToString(),
                                Remarks = reader["Remarks"].ToString(),
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }

                result.Success = true;
                result.Message = result.Data.Count > 0 ? "Exam results retrieved successfully." : "No results found.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static OperationResult<List<ExamResultDTO>> GetExamResultsByStudent(int studentId, string connectionString)
        {
            var result = new OperationResult<List<ExamResultDTO>> { Data = new List<ExamResultDTO>() };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetExamResultsByStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@studentId", studentId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Data.Add(new ExamResultDTO
                            {
                                ResultId = (int)reader["ResultId"],
                                ExamId = (int)reader["ExamId"],
                                ExamName = reader["ExamName"].ToString(),
                                ExamType = reader["ExamType"].ToString(),
                                EnrollmentId = (int)reader["enrollmentId"],
                                StudentName = reader["StudentName"].ToString(),
                                SubjectId = (int)reader["SubjectId"],
                                SubjectName = reader["SubjectName"].ToString(),
                                MarksObtained = (decimal)reader["MarksObtained"],
                                Grade = reader["Grade"].ToString(),
                                Remarks = reader["Remarks"].ToString(),
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }

                result.Success = true;
                result.Message = result.Data.Count > 0 ? "Exam results retrieved successfully." : "No results found.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static OperationResult<List<ExamResultDTO>> GetAllExamResults(string connectionString)
        {
            var result = new OperationResult<List<ExamResultDTO>> { Data = new List<ExamResultDTO>() };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllExamResults", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Data.Add(new ExamResultDTO
                            {
                                ResultId = (int)reader["ResultId"],
                                ExamId = (int)reader["ExamId"],
                                ExamName = reader["ExamName"].ToString(),
                                ExamType = reader["ExamType"].ToString(),
                                EnrollmentId = (int)reader["EnrollmentId"],
                                StudentName = reader["StudentName"].ToString(),
                                SubjectId = (int)reader["SubjectId"],
                                SubjectName = reader["SubjectName"].ToString(),
                                MarksObtained = (decimal)reader["MarksObtained"],
                                Grade = reader["Grade"].ToString(),
                                Remarks = reader["Remarks"].ToString(),
                                Status = reader["Status"].ToString()
                            });
                        }
                    }

                }

                result.Success = true;
                result.Message = result.Data.Count > 0 ? "Exam results retrieved successfully." : "No results found.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
    }
    public class FeeItemDal
    {
        public static OperationResult<FeeItemDTO> GetFeeItemById(int feeItemId, string connectionString)
        {
            var result = new OperationResult<FeeItemDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetFeeItemById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FeeItemId", feeItemId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Data = new FeeItemDTO
                            {
                                FeeItemId = (int)reader["FeeItemId"],
                                Name = reader["Name"].ToString(),
                                Amount = (decimal)reader["Amount"],
                                Frequency = reader["Frequency"].ToString(),
                                Status = reader["Status"].ToString()
                            };
                            result.Success = true;
                            result.Message = "Fee item found successfully.";
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = "Fee item not found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static OperationResult<List<FeeItemDTO>> GetAllActiveFeeItems(string connectionString)
        {
            var result = new OperationResult<List<FeeItemDTO>>()
            {
                Data = new List<FeeItemDTO>()
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllActiveFeeItems", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Data.Add(new FeeItemDTO
                            {
                                FeeItemId = (int)reader["FeeItemId"],
                                Name = reader["Name"].ToString(),
                                Amount = (decimal)reader["Amount"],
                                Frequency = reader["Frequency"].ToString(),
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }

                result.Success = result.Data.Count > 0;
                result.Message = result.Success ? "Fee items retrieved successfully." : "No fee items found.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static int CreateFeeItem(CreateFeeItemDTO feeItem, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_CreateFeeItem", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", feeItem.Name);
                    cmd.Parameters.AddWithValue("@Amount", feeItem.Amount);
                    cmd.Parameters.AddWithValue("@Frequency", feeItem.Frequency);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int UpdateFeeItem(UpdateFeeItemDTO feeItem, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdateFeeItem", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FeeItemId", feeItem.FeeItemId);
                    cmd.Parameters.AddWithValue("@Name", feeItem.Name);
                    cmd.Parameters.AddWithValue("@Amount", feeItem.Amount);
                    cmd.Parameters.AddWithValue("@Frequency", feeItem.Frequency);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int DeleteFeeItem(int feeItemId, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteFeeItem", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FeeItemId", feeItemId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
    }
    public class InvoiceDal
    {
        public static int DeleteInvoice(int invoiceId, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteInvoice", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -1;
            }
        }
        public static OperationResult<List<InvoiceDTO>> GetAllActiveInvoices(string connectionString)
        {
            var result = new OperationResult<List<InvoiceDTO>> { Data = new List<InvoiceDTO>() };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllActiveInvoices", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Data.Add(new InvoiceDTO
                            {
                                InvoiceId = (int)reader["InvoiceId"],
                                InvoiceNumber = reader["InvoiceNumber"].ToString(),
                                StudentId = (int)reader["StudentId"],
                                StudentName = reader["StudentName"].ToString(),
                                FeeItemName = reader["FeeItemName"].ToString(),
                                Amount = (decimal)reader["Amount"],
                                DueDate = (DateTime)reader["DueDate"],
                                Status = reader["Status"].ToString(),
                                BillingMonth = reader["BillingMonth"] as int?,
                                BillingYear = reader["BillingYear"] as int?,
                            });
                        }
                    }
                }

                result.Success = result.Data.Count > 0;
                result.Message = result.Success ? "Invoices retrieved successfully." : "No active invoices found.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static OperationResult<InvoiceDTO> GetInvoiceById(int invoiceId, string connectionString)
        {
            var result = new OperationResult<InvoiceDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetInvoiceById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Data = new InvoiceDTO
                            {
                                InvoiceId = (int)reader["InvoiceId"],
                                InvoiceNumber = reader["InvoiceNumber"].ToString(),
                                StudentId = (int)reader["StudentId"],
                                StudentName = reader["StudentName"].ToString(),
                                FeeItemName = reader["FeeItemName"].ToString(),
                                Amount = (decimal)reader["Amount"],
                                DueDate = (DateTime)reader["DueDate"],
                                Status = reader["Status"].ToString(),
                                BillingMonth = reader["BillingMonth"] as int?,
                                BillingYear = reader["BillingYear"] as int?,
                            };
                            result.Success = true;
                            result.Message = "Invoice found successfully.";
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = "Invoice not found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static OperationResult<List<InvoiceDTO>> GetInvoicesByStudentId(int studentId, string connectionString)
        {
            var result = new OperationResult<List<InvoiceDTO>> { Data = new List<InvoiceDTO>() };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetInvoicesByStudentId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Data.Add(new InvoiceDTO
                            {
                                InvoiceId = (int)reader["InvoiceId"],
                                InvoiceNumber = reader["InvoiceNumber"].ToString(),
                                StudentId = (int)reader["StudentId"],
                                StudentName = reader["StudentName"].ToString(),
                                FeeItemName = reader["FeeItemName"].ToString(),
                                Amount = (decimal)reader["Amount"],
                                DueDate = (DateTime)reader["DueDate"],
                                Status = reader["Status"].ToString(),
                                BillingMonth = reader["BillingMonth"] as int?,
                                BillingYear = reader["BillingYear"] as int?,
                            });
                        }
                    }
                }

                result.Success = result.Data.Count > 0;
                result.Message = result.Success ? "Invoices retrieved successfully." : "No invoices found for this student.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static int CreateInvoice(CreateInvoiceDTO invoice, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_CreateInvoice", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EnrollmentId", invoice.EnrollmentId);
                    cmd.Parameters.AddWithValue("@FeeItemId", invoice.FeeItemId);
                    cmd.Parameters.AddWithValue("@DueDate", invoice.DueDate);
                    cmd.Parameters.AddWithValue("@BillingMonth", invoice.BillingMonth);
                    cmd.Parameters.AddWithValue("@BillingYear", invoice.BillingYear);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int UpdateInvoice(UpdateInvoiceDTO invoice, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdateInvoice", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@InvoiceId", invoice.InvoiceId);
                    cmd.Parameters.AddWithValue("@DueDate", invoice.DueDate);
                    cmd.Parameters.AddWithValue("@Amount", invoice.Amount);
                    cmd.Parameters.AddWithValue("@Status", invoice.Status);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
    }
    public static class PaymentDal
    {
        public static int CreatePayment(CreatePaymentDTO payment, string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("sp_CreatePayment", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@InvoiceId", payment.InvoiceId);
                cmd.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
                cmd.Parameters.AddWithValue("@AmountPaid", payment.AmountPaid);
                cmd.Parameters.AddWithValue("@PaymentMethod", payment.PaymentMethod ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Notes", payment.Notes ?? (object)DBNull.Value);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                return -1; // System error
            }
        }
        public static int UpdatePayment(UpdatePaymentDTO payment, string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("sp_UpdatePayment", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PaymentId", payment.PaymentId);
                cmd.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
                cmd.Parameters.AddWithValue("@AmountPaid", payment.AmountPaid);
                cmd.Parameters.AddWithValue("@PaymentMethod", payment.PaymentMethod ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Notes", payment.Notes ?? (object)DBNull.Value);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                return -1;
            }
        }
        public static int DeletePayment(int paymentId, string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("sp_DeletePayment", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PaymentId", paymentId);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                return -1;
            }
        }
        public static OperationResult<PaymentDTO> GetPaymentById(int paymentId, string connectionString)
        {
            var result = new OperationResult<PaymentDTO>();

            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("sp_GetPaymentById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PaymentId", paymentId);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result.Data = new PaymentDTO
                    {
                        PaymentId = (int)reader["PaymentId"],
                        InvoiceId = (int)reader["InvoiceId"],
                        PaymentDate = (DateTime)reader["PaymentDate"],
                        AmountPaid = (decimal)reader["AmountPaid"],
                        PaymentMethod = reader["PaymentMethod"].ToString(),
                        Notes = reader["Notes"].ToString(),
                        PaymentStatus = reader["PaymentStatus"].ToString(),
                        InvoiceAmount = (decimal)reader["InvoiceAmount"],
                        InvoiceStatus = reader["InvoiceStatus"].ToString()
                    };
                    result.Success = true;
                    result.Message = "Payment found successfully.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Payment not found.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static OperationResult<List<PaymentDTO>> GetPaymentsByInvoiceId(int invoiceId, string connectionString)
        {
            var result = new OperationResult<List<PaymentDTO>> { Data = new List<PaymentDTO>() };

            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("sp_GetPaymentsByInvoiceId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Data.Add(new PaymentDTO
                    {
                        PaymentId = (int)reader["PaymentId"],
                        InvoiceId = (int)reader["InvoiceId"],
                        PaymentDate = (DateTime)reader["PaymentDate"],
                        AmountPaid = (decimal)reader["AmountPaid"],
                        PaymentMethod = reader["PaymentMethod"].ToString(),
                        Notes = reader["Notes"].ToString(),
                        PaymentStatus = reader["PaymentStatus"].ToString(),
                        InvoiceAmount = (decimal)reader["InvoiceAmount"],
                        InvoiceStatus = reader["InvoiceStatus"].ToString()
                    });
                }

                result.Success = result.Data.Count > 0;
                result.Message = result.Success ? "Payments retrieved successfully." : "No payments found.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static OperationResult<List<PaymentDTO>> GetPaymentsByStudentId(int studentId, string connectionString)
        {
            var result = new OperationResult<List<PaymentDTO>> { Data = new List<PaymentDTO>() };

            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("sp_GetPaymentsByStudentId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentId", studentId);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Data.Add(new PaymentDTO
                    {
                        PaymentId = (int)reader["PaymentId"],
                        InvoiceId = (int)reader["InvoiceId"],
                        PaymentDate = (DateTime)reader["PaymentDate"],
                        AmountPaid = (decimal)reader["AmountPaid"],
                        PaymentMethod = reader["PaymentMethod"].ToString(),
                        Notes = reader["Notes"].ToString(),
                        PaymentStatus = reader["PaymentStatus"].ToString(),
                        InvoiceAmount = (decimal)reader["InvoiceAmount"],
                        InvoiceStatus = reader["InvoiceStatus"].ToString()
                    });
                }

                result.Success = result.Data.Count > 0;
                result.Message = result.Success ? "Payments retrieved successfully." : "No payments found.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
    }
}


