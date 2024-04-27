using DevExpress.Maui.Controls;
using MobileClient.ViewModels;

namespace MobileClient.Views {
    public partial class OrdersPage : ContentPage {
        public OrdersPage(OrdersViewModel viewModel) {
            InitializeComponent();
            this.BindingContext = viewModel;
        }

        private void ordersCollection_Scrolled(object sender, DevExpress.Maui.CollectionView.DXCollectionViewScrolledEventArgs e) {
            if (detailInfoBottomSheet.HalfExpandedRatio == 0.5)
                detailInfoBottomSheet.Animate("detailInfoBottomSheet", x => detailInfoBottomSheet.HalfExpandedRatio = x, 0.5, 0.12);
        }

        private void ordersCollection_Tap(object sender, DevExpress.Maui.CollectionView.CollectionViewGestureEventArgs e) {
            if (detailInfoBottomSheet.HalfExpandedRatio == 0.12 && detailInfoBottomSheet.State == BottomSheetState.HalfExpanded) detailInfoBottomSheet.Animate("detailInfoBottomSheet", x => detailInfoBottomSheet.HalfExpandedRatio = x, 0.12, 0.5);
            else detailInfoBottomSheet.HalfExpandedRatio = 0.5;
            detailInfoBottomSheet.State = BottomSheetState.HalfExpanded;
        }

        private void OnGenerateReportClick(object sender, EventArgs e) {
            detailInfoBottomSheet.State = BottomSheetState.Hidden;
        }
    }
}