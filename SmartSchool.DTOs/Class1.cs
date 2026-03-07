using System;

namespace SmartSchool.DTOs
{
    // ======================= USERS =======================
    public class UserDTO
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public DateTime? RefreshTokenRevokedAt { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class CreateUserDTO
    {
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
    public class UpdateUserDTO
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
    }




    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }

    // ======================= STUDENTS =======================
    public class StudentDTO
    {
        public int StudentId { get; set; }
        public string? FullName { get; set; }
        public string? AdmissionNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public string? Status { get; set; }
    }
    public class CreateStudentDTO
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
    }
    public class UpdateStudentDTO
    {
        public int StudentId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
        public int SectionId { get; set; }
    }


    // ======================= TEACHERS =======================
    public class TeacherDTO
    {
        public int TeacherId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Specialization { get; set; }
        public DateTime HireDate { get; set; }
        public string? Status { get; set; }
    }
    public class CreateTeacherDTO
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Specialization { get; set; }
    }
    public class UpdateTeacherDTO
    {
        public int TeacherId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Specialization { get; set; }
        public DateTime HireDate { get; set; }
    }


    // ======================= SECTIONS =======================
    public class SectionDTO
    {
        public int SectionId { get; set; }
        public string? SectionName { get; set; }
        public int Capacity { get; set; }
        public string? ClassName { get; set; }
        public int TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string? Status { get; set; }
    }
    public class CreateSectionDTO
    {
        public int ClassId { get; set; }
        public string? SectionName { get; set; }
        public int Capacity { get; set; }
        public int HomeroomTeacherId { get; set; }
    }
    public class UpdateSectionDTO
    {
        public int SectionId { get; set; }
        public int ClassId { get; set; }
        public string? SectionName { get; set; }
        public int Capacity { get; set; }
        public int HomeroomTeacherId { get; set; }
    }



    // ======================= CLASSES =======================
    public class ClassDTO
    {
        public int ClassId { get; set; }
        public string? ClassName { get; set; }
        public int Level { get; set; }
    }


    // ======================= SUBJECTS =======================
    public class CreateSubjectDTO
    {
        public string? SubjectName { get; set; } = null!;
        public string? SubjectCode { get; set; } = null!;
        public int CreditHours { get; set; }
    }
    public class UpdateSubjectDTO
    {
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; } = null!;
        public string? SubjectCode { get; set; } = null!;
        public int CreditHours { get; set; }
    }
    public class SubjectDTO
    {
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public string? SubjectCode { get; set; }
        public int CreditHours { get; set; }
        public string? Status { get; set; }
    }




    // ======================= CLASSSUBJECTS =======================
    public class ClassSubjectDTO
    {
        public int ClassSubjectId { get; set; }
        public string? ClassName { get; set; }
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public string? Code { get; set; }
        public int CreditHours { get; set; }
    }
    public class CreateClassSubjectDTO
    {
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
    }




    // ======================= ENROLLMENTS =======================
    public class CreateEnrollmentDTO
    {
        public int StudentId { get; set; }
        public int SectionId { get; set; }
    }
    public class EnrollmentDTO
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public int SectionId { get; set; }
        public string? SectionName { get; set; }
        public string? ClassName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string? Status { get; set; }
    }
    public class UpdateEnrollmentDTO
    {
        public int EnrollmentId { get; set; }
        public int SectionId { get; set; }
    }




    // ======================= EXAMS =======================
    public class ExamDTO
    {
        public int AcademicYear { get; set; }
        public int ExamId { get; set; }
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
    }
    public class CreateExamDTO
    {
        public int AcademicYear { get; set; }
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Type { get; set; }
    }
    public class UpdateExamDTO
    {
        public int ExamId { get; set; }
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Type { get; set; }
        public int AcademicYear { get; set; }

    }



    // ======================= EXAMRESULTS =======================
    public class ExamResultDTO
    {
        public int ResultId { get; set; }
        public int ExamId { get; set; }
        public string? ExamName { get; set; }
        public string? ExamType { get; set; }
        public int EnrollmentId { get; set; }
        public string? StudentName { get; set; }
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public decimal MarksObtained { get; set; }
        public string? Grade { get; set; }
        public string? Remarks { get; set; }
        public string? Status { get; set; }
    }
    public class CreateExamResultDTO
    {
        public int ExamId { get; set; }
        public int EnrollmentId { get; set; }
        public int SubjectId { get; set; }
        public decimal MarksObtained { get; set; }
        public string? Remarks { get; set; }
    }
    public class UpdateExamResultDTO
    {
        public int ResultId { get; set; }
        public decimal MarksObtained { get; set; }
        public string? Remarks { get; set; }
    }


    // ======================= FEEITEMS =======================
    public class FeeItemDTO
    {
        public int FeeItemId { get; set; }
        public string? Name { get; set; }
        public decimal Amount { get; set; }
        public string? Frequency { get; set; }
        public string? Status { get; set; }
    }
    public class CreateFeeItemDTO
    {
        public string? Name { get; set; }
        public decimal Amount { get; set; }
        public string? Frequency { get; set; }
    }
    public class UpdateFeeItemDTO
    {
        public int FeeItemId { get; set; }
        public string? Name { get; set; }
        public decimal Amount { get; set; }
        public string? Frequency { get; set; }
    }



    // ======================= INVOICES =======================
    public class InvoiceDTO
    {
        public int InvoiceId { get; set; }
        public string? InvoiceNumber { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? FeeItemName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string? Status { get; set; }
        public int? BillingMonth { get; set; }
        public int? BillingYear { get; set; }
    }
    public class CreateInvoiceDTO
    {
        public int EnrollmentId { get; set; }
        public int FeeItemId { get; set; }
        public DateTime DueDate { get; set; }
        public int BillingMonth { get; set; }
        public int BillingYear { get; set; }
    }
    public class UpdateInvoiceDTO
    {
        public int InvoiceId { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
    }



    // ======================= PAYMENTS =======================
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int InvoiceId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Notes { get; set; }
        public string? PaymentStatus { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string? InvoiceStatus { get; set; }
    }
    public class CreatePaymentDTO
    {
        public int InvoiceId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Notes { get; set; }
    }
    public class UpdatePaymentDTO
    {
        public int PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Notes { get; set; }
    }
}

