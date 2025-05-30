using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAPTRINHWEB.Models
{
    public class Payment
    {
        [Key]
        [Column("ID_Payment")]
        public int ID_Payment { get; set; }
        
        [Required]
        [Column("ID_Booking")]
        public int ID_Booking { get; set; }
        
        [Column("Method")]
        public PaymentMethod Method { get; set; }
        
        [Required]
        [Column("Paid_Amount")]
        [DataType(DataType.Currency)]
        public decimal Paid_Amount { get; set; }
        
        [Required]
        [Column("Payment_Date")]
        public DateTime Payment_Date { get; set; }
        
        [Column("Status")]
        public PaymentStatus Status { get; set; } = PaymentStatus.Waiting;
        
        // Navigation properties
        [ForeignKey("ID_Booking")]
        public Booking Booking { get; set; }
    }
    
    public enum PaymentMethod
    {
        Cash = 0,
        MoMo = 1,
        VNPay = 2,
        BankTransfer = 3,
        CreditCard = 4
    }
    
    public enum PaymentStatus
    {
        Paid = 0,
        Failed = 1,
        Waiting = 2
    }
}