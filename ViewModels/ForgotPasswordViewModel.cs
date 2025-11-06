using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiFrontend.Services;
using System.Threading.Tasks;

namespace MauiFrontend.ViewModels
{
    public partial class ForgotPasswordViewModel : ObservableObject
    {
        private readonly UserService _userService;

        [ObservableProperty] private string email;
        [ObservableProperty] private string otp;
        [ObservableProperty] private string newPassword;

        [ObservableProperty] private bool isEmailStep = true;
        [ObservableProperty] private bool isOtpStep;
        [ObservableProperty] private bool isPasswordStep;

        public IAsyncRelayCommand SendOtpCommand { get; }
        public IAsyncRelayCommand VerifyOtpCommand { get; }
        public IAsyncRelayCommand ResetPasswordCommand { get; }
        public IAsyncRelayCommand BackToLoginCommand { get; }

        public ForgotPasswordViewModel(UserService userService)
        {
            _userService = userService;
            SendOtpCommand = new AsyncRelayCommand(SendOtp);
            VerifyOtpCommand = new AsyncRelayCommand(VerifyOtp);
            ResetPasswordCommand = new AsyncRelayCommand(ResetPassword);
            BackToLoginCommand = new AsyncRelayCommand(BackToLogin);
        }

        private async Task SendOtp()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                await Shell.Current.DisplayAlert("Lỗi", "Vui lòng nhập email!", "OK");
                return;
            }

            var success = await _userService.SendOtpAsync(Email);
            if (success)
            {
                await Shell.Current.DisplayAlert("Thành công", "Mã OTP đã được gửi qua email!", "OK");
                IsEmailStep = false;
                IsOtpStep = true;
            }
            else
            {
                await Shell.Current.DisplayAlert("Lỗi", "Không thể gửi mã OTP. Vui lòng thử lại.", "OK");
            }
        }

        private async Task VerifyOtp()
        {
            if (string.IsNullOrWhiteSpace(Otp))
            {
                await Shell.Current.DisplayAlert("Lỗi", "Vui lòng nhập mã OTP!", "OK");
                return;
            }

            var valid = await _userService.VerifyOtpAsync(Email, Otp);
            if (valid)
            {
                await Shell.Current.DisplayAlert("Xác nhận thành công", "Nhập mật khẩu mới để tiếp tục.", "OK");
                IsOtpStep = false;
                IsPasswordStep = true;
            }
            else
            {
                await Shell.Current.DisplayAlert("Sai OTP", "Mã OTP không hợp lệ hoặc đã hết hạn.", "OK");
            }
        }

        private async Task ResetPassword()
        {
            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                await Shell.Current.DisplayAlert("Lỗi", "Vui lòng nhập mật khẩu mới!", "OK");
                return;
            }

            var done = await _userService.ResetPasswordAsync(Email, NewPassword);
            if (done)
            {
                await Shell.Current.DisplayAlert("Thành công", "Mật khẩu đã được đặt lại.", "Đăng nhập");
                await Shell.Current.GoToAsync("///LoginPage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Lỗi", "Không thể đặt lại mật khẩu. Thử lại.", "OK");
            }
        }

        private async Task BackToLogin()
        {
            // Nếu đang ở bước nhập OTP hoặc mật khẩu → quay lại bước đầu
            if (IsOtpStep || IsPasswordStep)
            {
                IsEmailStep = true;
                IsOtpStep = false;
                IsPasswordStep = false;
                Otp = string.Empty;
                NewPassword = string.Empty;
            }
            else
            {
                await Shell.Current.GoToAsync("///LoginPage");
            }
        }

    }
}
