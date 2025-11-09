using MauiFrontend.Constants;
using MauiFrontend.Http;
using MauiFrontend.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MauiFrontend.Services
{
    public class UserService : BaseService
    {
        public UserService(Https https) : base(https) { }
      
        public async Task<LoginResponse?> LoginAsync(string email, string password)
        {
            try
            {
                var request = new LoginRequest
                {
                    email = email,
                    password = password
                };
              
                var response = await _https.PostAsync<LoginRequest, LoginResponse>(
                    APICONSTANT.USER.LOGIN,
                    request
                );
                 
                if (response != null )
                {
                    if(response.Code == 200)
                    {
                        if (!string.IsNullOrEmpty(response.Data?.Token))
                        {
                        
                            Preferences.Set("auth_token", response.Data.Token);
                            Preferences.Set("user_name", response.Data.UserName);
                            Preferences.Set("user_email", response.Data.Email);
                            Preferences.Set("user_roles", string.Join(",", response.Data.Roles));
                            if (Application.Current.MainPage is AppShell shell)
                            {
                                shell.UpdateAccountTitle(); 
                            }
                        }
                        return response;
                    }
                    else
                    {
                        return null;
                    }
                }

                System.Diagnostics.Debug.WriteLine($" Login failed: {response?.Message}");
                return null;
            }
            catch (Exception ex)
            {
               
               System.Diagnostics.Debug.WriteLine($" LoginAsync Exception: {ex.Message}");
                return null;
            }
        }
        // 🚪 Đăng xuất
        public void Logout()
        {
            Preferences.Remove("auth_token");
            Preferences.Remove("user_email");
            Preferences.Remove("user_roles");
        }
        //Kiểm tra đã đăng nhập chưa
        public bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(Preferences.Get("auth_token", null));
        }
        //Lấy token hiện tại
        public string GetToken()
        {
            return Preferences.Get("auth_token", null);
        }

        //  Lấy email người dùng
        public string GetUserEmail()
        {
            return Preferences.Get("user_email", null);
        }

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
