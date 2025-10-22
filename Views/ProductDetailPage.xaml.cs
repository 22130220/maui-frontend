using MauiFrontend.ViewModels;

namespace MauiFrontend.Views;

public partial class ProductDetailPage : ContentPage
{
    public ProductDetailPage(ProductDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        ShowTab(0);
    }

    void OnDescriptionClicked(object sender, EventArgs e) => ShowTab(0);
    void OnMaterialsClicked(object sender, EventArgs e) => ShowTab(1);
    void OnReviewsClicked(object sender, EventArgs e) => ShowTab(2);

    void ShowTab(int index)
    {
        // Ẩn toàn bộ content trước
        DescriptionContent.IsVisible = false;
        MaterialsContent.IsVisible = false;
        ReviewsContent.IsVisible = false;

        // Reset màu 3 nút
        ResetTabs();

        // Hiển thị tab tương ứng
        switch (index)
        {
            case 0:
                DescriptionContent.IsVisible = true;
                SetActiveButton(DescriptionBtn);
                break;
            case 1:
                MaterialsContent.IsVisible = true;
                SetActiveButton(MaterialsBtn);
                break;
            case 2:
                ReviewsContent.IsVisible = true;
                SetActiveButton(ReviewsBtn);
                break;
        }
    }

    void ResetTabs()
    {
        var defaultBg = Color.FromArgb("#FFF2D1");
        var defaultText = Color.FromArgb("#555");

        DescriptionBtn.BackgroundColor = defaultBg;
        DescriptionBtn.TextColor = defaultText;

        MaterialsBtn.BackgroundColor = defaultBg;
        MaterialsBtn.TextColor = defaultText;

        ReviewsBtn.BackgroundColor = defaultBg;
        ReviewsBtn.TextColor = defaultText;
    }

    void SetActiveButton(Button button)
    {
        button.BackgroundColor = Color.FromArgb("#F9A825");
        button.TextColor = Colors.White;
    }
}
