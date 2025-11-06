using MauiFrontend.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MauiFrontend.Services
{
    public class UserService : BaseService
    {
        public UserService(Https https) : base(https) { }

        // 📩 Gửi mã OTP đến email
        public async Task<bool> SendOtpAsync(string email)
        {
            try
            {
                var encodedEmail = WebUtility.UrlEncode(email);
                var response = await _https.PostAsync<object, HttpResponseMessage>(
                    $"/api/auth/forgot-password?email={encodedEmail}", null
                );

                if (response == null)
                {
                    System.Diagnostics.Debug.WriteLine("⚠️ SendOtpAsync: response null");
                    return false;
                }

                if (response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"✅ SendOtpAsync success for {email}");
                    return true;
                }

                var error = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"❌ SendOtpAsync failed: {error}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SendOtpAsync Exception: {ex.Message}");
                return false;
            }
        }



        // ✅ Xác thực mã OTP
        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            try
            {
                var encodedEmail = WebUtility.UrlEncode(email);
                var encodedOtp = WebUtility.UrlEncode(otp);

                var response = await _https.PostAsync<object, HttpResponseMessage>(
                    $"/api/auth/verify-otp?email={encodedEmail}&otp={encodedOtp}", null
                );

                if (response == null)
                {
                    System.Diagnostics.Debug.WriteLine("VerifyOtpAsync: response null");
                    return false;
                }

                if (response.IsSuccessStatusCode)
                    return true;

                var error = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"VerifyOtpAsync backend error: {error}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"VerifyOtpAsync exception: {ex.Message}");
                return false;
            }
        }

        // 🔑 Đặt lại mật khẩu mới
        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            try
            {
                var encodedEmail = WebUtility.UrlEncode(email);
                var encodedPass = WebUtility.UrlEncode(newPassword);

                var response = await _https.PostAsync<object, HttpResponseMessage>(
                    $"/api/auth/reset-password?email={encodedEmail}&newPassword={encodedPass}", null
                );

                if (response == null)
                {
                    System.Diagnostics.Debug.WriteLine("ResetPasswordAsync: response null");
                    return false;
                }

                if (response.IsSuccessStatusCode)
                    return true;

                var error = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"ResetPasswordAsync backend error: {error}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ResetPasswordAsync exception: {ex.Message}");
                return false;
            }
        }
    }
}
