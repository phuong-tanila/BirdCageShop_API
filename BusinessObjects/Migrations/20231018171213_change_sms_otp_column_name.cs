using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    public partial class change_sms_otp_column_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Sms_otp",
                newName: "SmsOtp");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "SmsOtp",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "otp_value",
                table: "SmsOtp",
                newName: "OtpValue");

            migrationBuilder.RenameColumn(
                name: "expired_at",
                table: "SmsOtp",
                newName: "ExpiredAt");

            migrationBuilder.RenameColumn(
                name: "create_at",
                table: "SmsOtp",
                newName: "CreateAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "SmsOtp",
                newName: "Sms_otp");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Sms_otp",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "OtpValue",
                table: "Sms_otp",
                newName: "otp_value");

            migrationBuilder.RenameColumn(
                name: "ExpiredAt",
                table: "Sms_otp",
                newName: "expired_at");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Sms_otp",
                newName: "create_at");
        }
    }
}
