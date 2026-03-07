using SmartSchool.DAL;
using SmartSchool.DTOs;
using System.Data;
using static SmartSchool.BLL.StudentBll;
namespace SmartSchool.BLL
{
    public static class UserBll
    {
        public static OperationResult<UserDTO> GetUserByEmail(string Email, string connectionString)
        {
            return UserDal.GetUserByEmail(Email, connectionString);
        }
        public static OperationResult<int> CreateUser(CreateUserDTO dto, string connectionString)
        {
            var result = new OperationResult<int>();
            try
            {
                dto.Role = dto.Role!.ToLower();
                if (string.IsNullOrWhiteSpace(dto.Username) ||
                    string.IsNullOrWhiteSpace(dto.Password) ||
                    (dto.Role != "admin" && dto.Role != "teacher" && dto.Role != "student"))
                {
                    result.Success = false;
                    result.Message = "Invalid input data.";
                    return result;
                }
                dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                int code = UserDal.CreateUser(dto, connectionString);

                switch (code)
                {
                    case > 0:
                        result.Success = true;
                        result.Message = "User created successfully.";
                        result.Data = code;
                        break;

                    case -2:
                        result.Success = false;
                        result.Message = "Unable to create user. Please check your information.";
                        break;


                    default:
                        result.Success = false;
                        result.Message = "System error occurred.";
                        break;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static OperationResult<bool> UpdateUser(UpdateUserDTO dto, string connectionString)
        {
            var result = new OperationResult<bool>();

            try
            {
                if (dto.UserId <= 0 ||
                    string.IsNullOrWhiteSpace(dto.Username) ||
                    string.IsNullOrWhiteSpace(dto.Role))
                {
                    result.Success = false;
                    result.Message = "Invalid input data.";
                    return result;
                }

                int code = UserDal.UpdateUser(dto, connectionString);

                switch (code)
                {
                    case 1:
                        result.Success = true;
                        result.Message = "User updated successfully.";
                        result.Data = true;
                        break;

                    case -2:
                        result.Success = false;
                        result.Message = "User not found.";
                        break;

                    case -4:
                        result.Success = false;
                        result.Message = "Unable to update user. Please check your information.";
                        break;

                    default:
                        result.Success = false;
                        result.Message = "System error occurred.";
                        break;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static OperationResult<bool> DeleteUser(int userId, string connectionString)
        {
            var result = new OperationResult<bool>();
            try
            {
                if (userId <= 0)
                {
                    result.Success = false;
                    result.Message = "Invalid user id.";
                    return result;
                }

                int code = UserDal.DeleteUser(userId, connectionString);
                switch (code)
                {
                    case 1:
                        result.Success = true;
                        result.Message = "User deleted successfully.";
                        result.Data = true;
                        break;

                    case -1:
                        result.Success = false;
                        result.Message = "User not found.";
                        break;


                    default:
                        result.Success = false;
                        result.Message = "System error occurred.";
                        break;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }
            return result;
        }
        public static OperationResult<UserDTO> GetUserById(int userId, string connectionString)
        {
            return UserDal.GetUserById(userId, connectionString);
        }
        public static OperationResult<List<UserDTO>> GetAllUsers(string connectionString)
        {
            return UserDal.GetAllUsers(connectionString);
        }
    }
    public class TeacherBll
    {
        public static (int Code, string Message) UpdateTeacher(UpdateTeacherDTO teacher, string connectionString)
        {
            int result = TeacherDal.UpdateTeacher(teacher, connectionString);

            return result switch
            {
                1 => (result, "Teacher updated successfully."),
                -2 => (result, "Email already exists."),
                -3 => (result, "Teacher not found."),
                -1 => (result, "System error occurred."),
                _ => (0, "Unknown error.")
            };
        }
        public static (int Code, string Message) DeleteTeacher(int teacherId, string connectionString)
        {
            int result = TeacherDal.DeleteTeacher(teacherId, connectionString);

            return result switch
            {
                1 => (result, "Teacher deleted successfully."),
                -2 => (result, "Teacher not found."),
                _ => (result, "Unknown error occurred.")
            };
        }
        public static (int Code, string Message) CreateTeacher(CreateTeacherDTO teacher, string connectionString)
        {
            int result = TeacherDal.CreateTeacher(teacher, connectionString);

            return result switch
            {
                > 0 => (result, "Teacher created successfully."),
                -1 => (result, "System error occurred."),
                -2 => (result, "Email already exists."),
                _ => (result, "Unknown error.")
            };
        }
        public static OperationResult<List<TeacherDTO>> GetAllActiveTeachers(string connectionString)
        {
            return TeacherDal.GetAllActiveTeachers(connectionString);
        }
        public static OperationResult<TeacherDTO> GetTeacherById(int teacherId, string connectionString)
        {
            return TeacherDal.GetTeacherById(teacherId, connectionString);
        }
    }
    public class StudentBll
    {
        public static (int Code, string Message) UpdateStudent(UpdateStudentDTO student, string connectionString)
        {
            int result = StudentDal.UpdateStudent(student, connectionString);

            return result switch
            {
                1 => (result, "Student updated successfully."),
                -2 => (result, "Email already exists."),
                -3 => (result, "Student not found."),
                -4 => (result, "Section not found."),
                -1 => (result, "System error occurred."),
                _ => (0, "Unknown error.")
            };
        }
        public static (int Code, string Message) DeleteStudent(int studentId, string connectionString)
        {
            int result = StudentDal.DeleteStudent(studentId, connectionString);

            return result switch
            {
                1 => (result, "Student deleted successfully."),
                -1 => (result, "A system error occurred."),
                -2 => (result, "Student not found."),
                _ => (result, "Unknown error occurred.")
            };
        }
        public static (int Code, string Message) CreateStudent(CreateStudentDTO student, string connectionString)
        {
            int result = StudentDal.CreateStudent(student, connectionString);

            return result switch
            {
                > 0 => (result, "Student created successfully."),
                -1 => (result, "A system error occurred while creating the student."),
                -2 => (result, "Email already exists."),
                _ => (result, "Unknown error occurred.")
            };
        }
        public static OperationResult<List<StudentDTO>> GetAllActiveStudents(string connectionString)
        {
            return StudentDal.GetAllActiveStudents(connectionString);
        }
        public static OperationResult<StudentDTO> GetStudentById(int studentId, string connectionString)
        {
            return StudentDal.GetStudentById(studentId, connectionString);
        }
    }
    public class SubjectBll
    {
        public static (int Code, string Message) CreateSubject(CreateSubjectDTO subject, string connectionString)
        {
            int result = SubjectDal.CreateSubject(subject, connectionString);
            return result switch
            {
                > 0 => (result, "Subject created successfully."),
                -2 => (result, "Subject code and Subject name already exists."),
                _ => (result, "Unknown error occurred.")
            };
        }
        public static (int Code, string Message) UpdateSubject(UpdateSubjectDTO subject, string connectionString)
        {
            int result = SubjectDal.UpdateSubject(subject, connectionString);
            return result switch
            {
                1 => (result, "Subject updated successfully."),
                -2 => (result, "Subject code already exists."),
                -3 => (result, "Subject not found."),
                _ => (result, "Unknown error.")
            };
        }
        public static (int Code, string Message) DeleteSubject(int subjectId, string connectionString)
        {
            int result = SubjectDal.DeleteSubject(subjectId, connectionString);
            return result switch
            {
                1 => (result, "Subject deleted successfully."),
                -2 => (result, "Subject is linked and cannot be deleted."),
                -3 => (result, "Subject not found."),
                _ => (result, "Unknown error occurred.")
            };
        }
        public static OperationResult<SubjectDTO> GetSubjectById(int subjectId, string connectionString)
        {
            return SubjectDal.GetSubjectById(subjectId, connectionString);
        }
        public static OperationResult<List<SubjectDTO>> GetAllActiveSubjects(string connectionString)
        {
            return SubjectDal.GetAllActiveSubjects(connectionString);
        }
    }
    public class SectionBll
    {
        public static (int Code, string Message) CreateSection(CreateSectionDTO section, string connectionString)
        {
            int result = SectionDal.CreateSection(section, connectionString);

            return result switch
            {
                > 0 => (result, "Section created successfully."),
                -2 => (result, "Class not found."),
                -3 => (result, "Homeroom teacher not found."),
                -4 => (result, "Section name already exists for this class."),
                _ => (result, "A system error occurred while creating the section.")
            };
        }
        public static (int Code, string Message) UpdateSection(UpdateSectionDTO section, string connectionString)
        {
            int result = SectionDal.UpdateSection(section, connectionString);

            return result switch
            {
                1 => (result, "Section updated successfully."),
                -2 => (result, "Section not found."),
                -3 => (result, "Class not found."),
                -4 => (result, "Homeroom teacher not found."),
                -5 => (result, "Section name already exists for this class."),
                _ => (0, "Unknown error occurred.")
            };
        }
        public static (int Code, string Message) DeleteSection(int sectionId, string connectionString)
        {
            int result = SectionDal.DeleteSection(sectionId, connectionString);

            return result switch
            {
                1 => (result, "Section deactivated successfully."),
                -2 => (result, "Section not found."),
                -3 => (result, "Cannot delete section because it has enrolled students."),
                _ => (result, "Unknown error occurred.")
            };
        }
        public static OperationResult<SectionDTO> GetSectionById(int sectionId, string connectionString)
        {
            return SectionDal.GetSectionById(sectionId, connectionString);
        }
        public static OperationResult<List<SectionDTO>> GetAllActiveSections(string connectionString)
        {
            return SectionDal.GetAllActiveSections(connectionString);
        }
    }
    public class ClassBll
    {
        public static OperationResult<List<ClassDTO>> GetAllClasses(string connectionString)
        {
            return ClassDal.GetAllClasses(connectionString);
        }
    }
    public class ClassSubjectBll
    {
        public static (int Code, string Message) CreateClassSubject(CreateClassSubjectDTO dto, string connectionString)
        {
            int result = ClassSubjectDal.CreateClassSubject(dto, connectionString);

            return result switch
            {
                > 0 => (result, "Subject linked to class successfully."),
                -2 => (result, "Subject already linked to this class."),
                -3 => (result, "Class not found."),
                -4 => (result, "Subject not found."),
                _ => (result, "Unknown error occurred.")
            };
        }
        public static OperationResult<List<ClassSubjectDTO>> GetClassSubjectsByClass(int classId, string connectionString)
        {
            return ClassSubjectDal.GetClassSubjectsByClass(classId, connectionString);
        }
        public static (int Code, string Message) DeleteClassSubject(int classSubjectId, string connectionString)
        {
            int result = ClassSubjectDal.DeleteClassSubject(classSubjectId, connectionString);

            return result switch
            {
                1 => (result, "Subject unlinked successfully."),
                -2 => (result, "Link not found."),
                _ => (result, "Unknown error occurred.")
            };
        }
    }
    public class EnrollmentBll
    {
        public static (int Code, string Message) UpdateEnrollment(UpdateEnrollmentDTO enrollment, string connectionString)
        {
            int result = EnrollmentDal.UpdateEnrollment(enrollment, connectionString);

            return result switch
            {
                1 => (result, "Enrollment updated successfully."),
                -2 => (result, "Enrollment not found."),
                -3 => (result, "New section not found."),
                -4 => (result, "Student is already enrolled in this section."),
                -5 => (result, "The target section is full."),
                _ => (0, "Unknown error.")
            };
        }
        public static (int Code, string Message) CreateEnrollment(CreateEnrollmentDTO enrollment, string connectionString)
        {
            int result = EnrollmentDal.CreateEnrollment(enrollment, connectionString);
            return result switch
            {
                > 0 => (result, "Enrollment created successfully."),
                -2 => (result, "Student not found."),
                -3 => (result, "Section not found."),
                -4 => (result, "Student already enrolled in this section."),
                -5 => (result, "The section is full and cannot accept more students."),
                _ => (result, "Unknown error.")
            };
        }
        public static (int Code, string Message) DeleteEnrollment(int enrollmentId, string connectionString)
        {
            int result = EnrollmentDal.DeleteEnrollment(enrollmentId, connectionString);
            return result switch
            {
                1 => (result, "Enrollment deleted successfully."),
                -2 => (result, "Enrollment not found."),
                -3 => (result, "Enrollment already deleted."),
                _ => (result, "Unknown error.")
            };
        }
        public static OperationResult<List<EnrollmentDTO>> GetAllEnrollments(string connectionString)
        {
            return EnrollmentDal.GetAllEnrollments(connectionString);
        }
        public static OperationResult<List<EnrollmentDTO>> GetEnrollmentsByStudent(int studentId, string connectionString)
        {
            return EnrollmentDal.GetEnrollmentsByStudent(studentId, connectionString);
        }
        public static OperationResult<List<EnrollmentDTO>> GetStudentsBySection(int sectionId, string connectionString)
        {
            return EnrollmentDal.GetStudentsBySection(sectionId, connectionString);
        }
    }
    public class ExamBll
    {
        public static (int Code, string Message) CreateExam(CreateExamDTO exam, string connectionString)
        {
            int result = ExamDal.CreateExam(exam, connectionString);
            return result switch
            {
                > 0 => (result, "Exam created successfully."),
                -2 => (result, "Exam with same name and type and academicYear already exists."),
                -3 => (result, "StartDate cannot be after EndDate."),
                _ => (result, "System error occurred.")
            };
        }
        public static (int Code, string Message) UpdateExam(UpdateExamDTO exam, string connectionString)
        {
            int result = ExamDal.UpdateExam(exam, connectionString);
            return result switch
            {
                1 => (result, "Exam updated successfully."),
                -2 => (result, "Exam not found."),
                -3 => (result, "Exam with same name and type and academicYear already exists."),
                -4 => (result, "StartDate cannot be after EndDate."),
                _ => (result, "System error occurred.")
            };
        }
        public static (int Code, string Message) DeleteExam(int examId, string connectionString)
        {
            int result = ExamDal.DeleteExam(examId, connectionString);
            return result switch
            {
                1 => (result, "Exam deleted successfully."),
                -3 => (result, "Exam already inactive."),
                -2 => (result, "Exam not found."),
                _ => (result, "System error occurred.")
            };
        }
        public static OperationResult<List<ExamDTO>> GetAllActiveExams(string connectionString)
        {
            return ExamDal.GetAllActiveExams(connectionString);
        }
    }
    public class ExamResultBll
    {
        public static (int Code, string Message) CreateExamResult(CreateExamResultDTO result, string connectionString)
        {
            int res = ExamResultDal.CreateExamResult(result, connectionString);
            return res switch
            {
                1 => (res, "Exam result created successfully."),
                -2 => (res, "Exam not found."),
                -3 => (res, "Enrollment not found."),
                -4 => (res, "Subject not found."),
                -5 => (res, "Marks obtained are invalid."),
                -6 => (res, "Exam result already exists."),
                _ => (res, "Unknown error occurred.")
            };
        }
        public static (int Code, string Message) UpdateExamResult(UpdateExamResultDTO result, string connectionString)
        {
            int res = ExamResultDal.UpdateExamResult(result, connectionString);
            return res switch
            {
                1 => (res, "Exam result updated successfully."),
                -2 => (res, "Exam result not found."),
                -3 => (res, "Marks obtained are invalid."),
                _ => (res, "Unknown error occurred.")
            };
        }
        public static (int Code, string Message) DeleteExamResult(int resultId, string connectionString)
        {
            int res = ExamResultDal.DeleteExamResult(resultId, connectionString);
            return res switch
            {
                1 => (res, "Exam result deleted successfully."),
                -1 => (res, "Exam result not found."),
                _ => (res, "Unknown error occurred.")
            };
        }
        public static OperationResult<List<ExamResultDTO>> GetExamResultsByExam(int examId, string connectionString)
        {
            return ExamResultDal.GetExamResultsByExam(examId, connectionString);
        }
        public static OperationResult<List<ExamResultDTO>> GetExamResultsByStudent(int studentId, string connectionString)
        {
            return ExamResultDal.GetExamResultsByStudent(studentId, connectionString);
        }
        public static OperationResult<List<ExamResultDTO>> GetAllExamResults(string connectionString)
        {
            return ExamResultDal.GetAllExamResults(connectionString);
        }
    }
    public class FeeItemBll
    {
        public static OperationResult<List<FeeItemDTO>> GetAllActiveFeeItems(string connectionString)
        {
            return FeeItemDal.GetAllActiveFeeItems(connectionString);
        }

        public static OperationResult<FeeItemDTO> GetFeeItemById(int feeItemId, string connectionString)
        {
            return FeeItemDal.GetFeeItemById(feeItemId, connectionString);
        }
        public static (int Code, string Message) CreateFeeItem(CreateFeeItemDTO feeItem, string connectionString)
        {
            int result = FeeItemDal.CreateFeeItem(feeItem, connectionString);

            return result switch
            {
                > 0 => (result, "Fee item created successfully."),
                -2 => (result, "Fee item name already exists."),
                -4 => (result, "Fee item reactivated successfully."),
                _ => (result, "Unknown error occurred.")
            };
        }

        public static (int Code, string Message) UpdateFeeItem(UpdateFeeItemDTO feeItem, string connectionString)
        {
            int result = FeeItemDal.UpdateFeeItem(feeItem, connectionString);

            return result switch
            {
                1 => (result, "Fee item updated successfully."),
                -1 => (result, "Fee item not found."),
                -5 => (result, "Another fee item with the same name already exists."),
                _ => (result, "Unknown error occurred.")
            };
        }

        public static (int Code, string Message) DeleteFeeItem(int feeItemId, string connectionString)
        {
            int result = FeeItemDal.DeleteFeeItem(feeItemId, connectionString);

            return result switch
            {
                1 => (result, "Fee item deleted successfully."),
                -1 => (result, "Fee item not found."),
                -2 => (result, "Cannot delete fee item. There are invoices linked to it."),
                _ => (result, "Unknown error occurred.")
            };
        }
    }
    public class InvoiceBll
    {
        public static (int Code, string Message) DeleteInvoice(int invoiceId, string connectionString)
        {
            int result = InvoiceDal.DeleteInvoice(invoiceId, connectionString);

            return result switch
            {
                1 => (result, "Invoice deleted successfully."),
                -2 => (result, "Invoice not found or already inactive."),
                -3 => (result, "Cannot delete invoice: payments exist."),
                _ => (result, "Unknown error occurred.")
            };
        }
        public static (int Code, string Message) UpdateInvoice(UpdateInvoiceDTO invoice, string connectionString)
        {
            int result = InvoiceDal.UpdateInvoice(invoice, connectionString);

            return result switch
            {
                1 => (result, "Invoice updated successfully."),
                -2 => (result, "Invoice not found or inactive."),
                -3 => (result, "Cannot modify a fully paid invoice."),
                -4 => (result, "Cannot set status to Paid: total payments do not match invoice amount."),
                _ => (result, "Unknown error occurred.")
            };
        }
        public static (int Code, string Message) CreateInvoice(CreateInvoiceDTO invoice, string connectionString)
        {
            int result = InvoiceDal.CreateInvoice(invoice, connectionString);

            return result switch
            {
                > 0 => (result, "Invoice created successfully."),
                -2 => (result, "An active invoice already exists for this month and fee item."),
                -3 => (result, "Enrollment or FeeItem not found."),
                -4 => (result, "FeeItem not found."),
                _ => (result, "Unknown error occurred.")
            };
        }
        public static OperationResult<List<InvoiceDTO>> GetAllActiveInvoices(string connectionString)
        {
            return InvoiceDal.GetAllActiveInvoices(connectionString);
        }
        public static OperationResult<InvoiceDTO> GetInvoiceById(int invoiceId, string connectionString)
        {
            return InvoiceDal.GetInvoiceById(invoiceId, connectionString);
        }
        public static OperationResult<List<InvoiceDTO>> GetInvoicesByStudentId(int studentId, string connectionString)
        {
            return InvoiceDal.GetInvoicesByStudentId(studentId, connectionString);
        }
    }
    public static class PaymentBll
    {
        public static (int Code, string Message) CreatePayment(CreatePaymentDTO payment, string connectionString)
        {
            int result = PaymentDal.CreatePayment(payment, connectionString);

            return result switch
            {
                1 => (result, "Payment created successfully."),
                -2 => (result, "Invoice not found or inactive."),
                -3 => (result, "Invoice already fully paid."),
                -4 => (result, "Payment exceeds remaining invoice amount."),
                -1 => (result, "System error occurred."),
                _ => (result, "Unknown error occurred.")
            };
        }

        public static (int Code, string Message) UpdatePayment(UpdatePaymentDTO payment, string connectionString)
        {
            int result = PaymentDal.UpdatePayment(payment, connectionString);

            return result switch
            {
                1 => (result, "Payment updated successfully."),
                -2 => (result, "Payment not found or inactive."),
                -3 => (result, "Payment exceeds remaining invoice amount."),
                -1 => (result, "System error occurred."),
                _ => (result, "Unknown error occurred.")
            };
        }

        public static (int Code, string Message) DeletePayment(int paymentId, string connectionString)
        {
            int result = PaymentDal.DeletePayment(paymentId, connectionString);

            return result switch
            {
                1 => (result, "Payment deleted successfully."),
                -2 => (result, "Payment not found or already inactive."),
                -1 => (result, "System error occurred."),
                _ => (result, "Unknown error occurred.")
            };
        }

        public static OperationResult<PaymentDTO> GetPaymentById(int paymentId, string connectionString)
        {
            return PaymentDal.GetPaymentById(paymentId, connectionString);
        }

        public static OperationResult<List<PaymentDTO>> GetPaymentsByInvoiceId(int invoiceId, string connectionString)
        {
            return PaymentDal.GetPaymentsByInvoiceId(invoiceId, connectionString);
        }
        public static OperationResult<List<PaymentDTO>> GetPaymentsByStudentId(int studentId, string connectionString)
        {
            return PaymentDal.GetPaymentsByStudentId(studentId, connectionString);
        }
    }

}
