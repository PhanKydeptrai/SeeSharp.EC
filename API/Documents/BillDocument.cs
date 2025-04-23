using Application.DTOs.Bills;
using Application.DTOs.Order;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace API.Documents;

public class BillDocument : IDocument
{
    private readonly BillResponse _bill;
    private static string LogoPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "images", "logo.png");
    
    // Defining theme colors
    private static readonly Color PrimaryColor = Colors.Blue.Medium;
    private static readonly Color SecondaryColor = Colors.Grey.Lighten1;
    private static readonly Color AccentColor = Colors.Teal.Medium;
    private static readonly Color TextColor = Colors.Grey.Darken4;
    private static readonly Color BorderColor = Colors.Grey.Lighten3;
    private static readonly Color PrimaryColorLight = Colors.Blue.Lighten1;
    
    public BillDocument(BillResponse bill)
    {
        _bill = bill;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);
                
                // Set default text style
                page.DefaultTextStyle(x => x.FontSize(10).FontColor(TextColor));
                
                // Add decorative elements
                page.Header()
                    .PaddingBottom(5)
                    .Column(column => 
                    {
                        // Top blue bar
                        column.Item().Height(12).Background(PrimaryColor);
                    });
                
                page.Content().Element(ComposeContent);
                
                page.Footer()
                    .PaddingTop(10)
                    .Column(column =>
                    {
                        // Footer content first
                        column.Item().Element(ComposeFooter);
                        
                        // Bottom blue bar
                        column.Item().Height(20).Background(PrimaryColorLight);
                    });
            });
    }

    private void ComposeHeader(IContainer container)
    {
        container.PaddingTop(20).Row(row =>
        {
            // Logo
            if (File.Exists(LogoPath))
            {
                row.ConstantItem(120).Image(LogoPath).FitArea();
            }
            else
            {
                row.ConstantItem(120).Column(column =>
                {
                    column.Item().Background(PrimaryColor).Padding(10)
                        .Text("SeeSharp.EC").FontSize(20).Bold().FontColor(Colors.White);
                });
            }

            row.RelativeItem().Padding(10).Column(column =>
            {
                column.Item().Text("SeeSharp.EC").FontSize(22).Bold().FontColor(PrimaryColor);
                column.Item().Text("Hóa đơn mua hàng").FontSize(14).FontColor(AccentColor);
                column.Item().PaddingTop(5);
                column.Item().LineHorizontal(1).LineColor(BorderColor);
                column.Item().PaddingTop(5);
                column.Item().Text($"Mã hóa đơn: {_bill.BillId}").FontSize(10).FontColor(TextColor);
                column.Item().Text($"Ngày: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(10).FontColor(TextColor);
            });
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.Column(column =>
        {
            // Header content
            column.Item().Element(ComposeHeader);
            
            column.Item().PaddingVertical(20).Column(contentColumn =>
            {
                // Customer information
                contentColumn.Item().PaddingVertical(5).Element(ComposeCustomerInfo);
                
                // Order details
                contentColumn.Item().PaddingVertical(10).Element(ComposeOrderDetails);
                
                // Total summary
                contentColumn.Item().PaddingVertical(5).Element(ComposeSummary);
            });
        });
    }

    private void ComposeCustomerInfo(IContainer container)
    {
        container.Border(1).BorderColor(BorderColor).Background(Colors.Grey.Lighten5)
            .Padding(15).Column(column =>
        {
            column.Item().Row(row =>
            {
                row.RelativeItem().Text("THÔNG TIN KHÁCH HÀNG").FontSize(14).Bold().FontColor(PrimaryColor);
                row.ConstantItem(60).Text("👤").FontSize(30).FontColor(PrimaryColor).AlignCenter();
            });
            
            column.Item().PaddingTop(5).LineHorizontal(1).LineColor(BorderColor);
            column.Item().PaddingTop(5);
            
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(100);
                    columns.RelativeColumn();
                });
                
                table.Cell().Text("Tên:").Bold().FontColor(TextColor);
                table.Cell().Text(_bill.UserName ?? "Không có thông tin").FontColor(TextColor);
                
                table.Cell().Text("Số điện thoại:").Bold().FontColor(TextColor);
                table.Cell().Text(_bill.PhoneNumber ?? "Không có thông tin").FontColor(TextColor);
                
                table.Cell().Text("Địa chỉ:").Bold().FontColor(TextColor);
                table.Cell().Text(_bill.SpecificAddress ?? "Không có thông tin").FontColor(TextColor);
            });
        });
    }

    private void ComposeOrderDetails(IContainer container)
    {
        container.Border(1).BorderColor(BorderColor).Padding(15).Column(column =>
        {
            // Title with decorative element
            column.Item().Row(row =>
            {
                row.ConstantItem(10).Background(AccentColor);
                row.RelativeItem().Padding(5).Text("CHI TIẾT ĐƠN HÀNG").FontSize(14).Bold().FontColor(PrimaryColor);
            });
            
            column.Item().PaddingTop(5).LineHorizontal(1).LineColor(BorderColor);
            column.Item().Height(10);
            
            // Table header
            column.Item().Table(table =>
            {
                // Define columns
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3);    // Product name
                    columns.RelativeColumn(2);    // Variant & Color
                    columns.RelativeColumn(1);    // Quantity
                    columns.RelativeColumn(2);    // Unit price
                    columns.RelativeColumn(2);    // Total
                });

                // Header row
                table.Header(header =>
                {
                    header.Cell().Background(PrimaryColorLight).Padding(5)
                        .Text("Sản phẩm").FontColor(TextColor).Bold();
                    header.Cell().Background(PrimaryColorLight).Padding(5)
                        .Text("Loại / Màu").FontColor(TextColor).Bold();
                    header.Cell().Background(PrimaryColorLight).Padding(5)
                        .Text("SL").FontColor(TextColor).Bold().AlignCenter();
                    header.Cell().Background(PrimaryColorLight).Padding(5)
                        .Text("Đơn giá").FontColor(TextColor).Bold().AlignRight();
                    header.Cell().Background(PrimaryColorLight).Padding(5)
                        .Text("Thành tiền").FontColor(TextColor).Bold().AlignRight();
                });

                // Data rows
                for (var i = 0; i < _bill.OrderDetails.Length; i++)
                {
                    var item = _bill.OrderDetails[i];
                    var backgroundColor = i % 2 == 0 ? Colors.White : Colors.Grey.Lighten5;
                    
                    table.Cell().Background(backgroundColor).Padding(5).Text(item.ProductName).FontColor(TextColor);
                    table.Cell().Background(backgroundColor).Padding(5).Text($"{item.VariantName} / {item.ColorCode}").FontColor(TextColor);
                    table.Cell().Background(backgroundColor).Padding(5).Text(item.Quantity.ToString()).FontColor(TextColor).AlignCenter();
                    table.Cell().Background(backgroundColor).Padding(5).Text($"{item.Price:N0} VNĐ").FontColor(TextColor).AlignRight();
                    table.Cell().Background(backgroundColor).Padding(5).Text($"{item.Total:N0} VNĐ").FontColor(TextColor).AlignRight();
                }
            });
        });
    }

    private void ComposeSummary(IContainer container)
    {
        container.Border(1).BorderColor(BorderColor).Padding(15).Column(column =>
        {
            // Title with decorative element
            column.Item().Row(row =>
            {
                row.ConstantItem(10).Background(AccentColor);
                row.RelativeItem().Padding(5).Text("TỔNG THANH TOÁN").FontSize(14).Bold().FontColor(PrimaryColor);
            });
            
            column.Item().PaddingTop(5).LineHorizontal(1).LineColor(BorderColor);
            column.Item().Height(10);
            
            column.Item().Row(row =>
            {
                row.RelativeItem(3).Text("Tổng tiền hàng:").FontColor(TextColor);
                row.RelativeItem(2).Text($"{_bill.Total:N0} VNĐ").FontColor(TextColor).AlignRight();
            });

            if (!string.IsNullOrEmpty(_bill.VoucherCode))
            {
                column.Item().PaddingTop(5).Row(row =>
                {
                    row.RelativeItem(3).Text($"Mã giảm giá: {_bill.VoucherCode}").FontColor(TextColor);
                    row.RelativeItem(2).Text($"-{(_bill.Total - _bill.BillTotal):N0} VNĐ").FontColor(Colors.Red.Medium).AlignRight();
                });
                
                column.Item().PaddingTop(5).LineHorizontal(1).LineColor(BorderColor);
            }

            column.Item().PaddingTop(5).Row(row =>
            {
                row.RelativeItem(3).Text("Thành tiền:").Bold().FontColor(PrimaryColor).FontSize(12);
                row.RelativeItem(2).Background(Colors.Grey.Lighten4).Padding(5)
                    .Text($"{_bill.BillTotal:N0} VNĐ").Bold().FontColor(PrimaryColor).FontSize(12).AlignRight();
            });

            column.Item().PaddingTop(15).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });
                
                // Payment info
                table.Cell().Border(1).BorderColor(BorderColor).Background(Colors.Grey.Lighten5).Padding(10)
                    .Column(column =>
                    {
                        column.Item().Text("PHƯƠNG THỨC THANH TOÁN").Bold().FontSize(10).FontColor(TextColor);
                        column.Item().PaddingTop(5);
                        column.Item().Text(_bill.PaymentMethod).FontColor(TextColor);
                    });
                
                // Status info
                table.Cell().Border(1).BorderColor(BorderColor).Background(Colors.Grey.Lighten5).Padding(10)
                    .Column(column =>
                    {
                        column.Item().Text("TRẠNG THÁI").Bold().FontSize(10).FontColor(TextColor);
                        column.Item().PaddingTop(5);
                        column.Item().Background(GetStatusColor(_bill.PaymentStatus)).PaddingVertical(3).PaddingHorizontal(5)
                            .Text(_bill.PaymentStatus).FontColor(Colors.White).AlignCenter();
                    });
            });
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().AlignCenter().Text("Cảm ơn quý khách đã mua hàng tại SeeSharp.EC")
                .FontSize(12).Bold().FontColor(PrimaryColor);
                
            column.Item().AlignCenter().Text("Mọi thắc mắc xin liên hệ: support@seesharp.ec | 1900-1234")
                .FontSize(10).FontColor(TextColor);
                
            column.Item().PaddingTop(10).LineHorizontal(1).LineColor(BorderColor);
            
            column.Item().PaddingTop(10).AlignCenter().Text("© 2024 SeeSharp.EC - All rights reserved")
                .FontSize(9).FontColor(SecondaryColor);
        });
    }
    
    private static Color GetStatusColor(string status)
    {
        return status.ToLower() switch
        {
            "completed" or "success" or "thành công" or "hoàn thành" => Colors.Green.Medium,
            "pending" or "chờ xử lý" => Colors.Orange.Medium,
            "failed" or "thất bại" => Colors.Red.Medium,
            _ => Colors.Blue.Medium
        };
    }
} 