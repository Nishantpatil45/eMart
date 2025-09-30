# Order Management System

## Overview
The Order Management System provides comprehensive functionality for handling e-commerce orders, including order creation, tracking, status management, and analytics.

## Features

### Core Functionality
- **Order Creation**: Create orders with multiple items
- **Order Tracking**: Track orders by ID or order number
- **Status Management**: Update order and payment statuses
- **Order History**: View order history for buyers and sellers
- **Order Cancellation**: Cancel orders with automatic stock restoration
- **Analytics**: Get order statistics and sales data

### Order Status Flow
1. **Pending** → Order created, awaiting confirmation
2. **Confirmed** → Order confirmed by seller
3. **Processing** → Order being prepared
4. **Shipped** → Order shipped to customer
5. **Delivered** → Order delivered successfully
6. **Cancelled** → Order cancelled (by buyer or seller)
7. **Refunded** → Order refunded

### Payment Status Flow
1. **Pending** → Payment not yet processed
2. **Paid** → Payment successful
3. **Failed** → Payment failed
4. **Refunded** → Full refund processed
5. **Partially Refunded** → Partial refund processed

## API Endpoints

### Order Management
- `POST /api/order` - Create a new order
- `GET /api/order/{id}` - Get order by ID
- `GET /api/order/number/{orderNumber}` - Get order by order number
- `GET /api/order/my-orders` - Get orders for logged-in user
- `GET /api/order` - Get all orders (admin only)
- `GET /api/order/seller/{sellerId}` - Get orders for specific seller (admin only)

### Order Status Management
- `PUT /api/order/{id}/status` - Update order status (admin only)
- `PUT /api/order/{id}/payment-status` - Update payment status (admin only)
- `PUT /api/order/{id}/cancel` - Cancel order (buyer only)

### Analytics
- `GET /api/order/statistics` - Get order statistics (admin only)

## Data Models

### Order
```csharp
public class Order
{
    public string Id { get; set; }
    public string OrderNumber { get; set; }
    public string BuyerId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingAmount { get; set; }
    public string Status { get; set; }
    public string PaymentStatus { get; set; }
    public string PaymentMethod { get; set; }
    public string? ShippingAddress { get; set; }
    public string? BillingAddress { get; set; }
    public string? Notes { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}
```

### OrderItem
```csharp
public class OrderItem
{
    public string Id { get; set; }
    public string OrderId { get; set; }
    public string ProductId { get; set; }
    public string SellerId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string? ProductName { get; set; }
    public string? ProductImageUrl { get; set; }
    public string? ProductDescription { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

## Business Logic

### Order Creation
1. Validate all order items and check product availability
2. Calculate total amount including tax and shipping
3. Generate unique order number
4. Create order and order items
5. Update product stock levels
6. Return created order details

### Order Cancellation
1. Verify user has permission to cancel order
2. Check if order can be cancelled (not delivered)
3. Update order status to cancelled
4. Restore product stock levels
5. Record cancellation timestamp

### Stock Management
- Stock is automatically reduced when order is created
- Stock is restored when order is cancelled
- Stock validation prevents overselling

## Security & Authorization

### Role-Based Access Control
- **Buyers**: Can create orders, view their own orders, cancel their orders
- **Sellers**: Can view orders for their products (via admin endpoints)
- **Admins**: Full access to all order management features

### Data Protection
- Users can only access their own orders
- Admin endpoints require admin role
- All endpoints require authentication

## Error Handling

### Common Error Scenarios
- **Insufficient Stock**: Returns 400 Bad Request with stock details
- **Product Not Found**: Returns 400 Bad Request
- **Order Not Found**: Returns 404 Not Found
- **Unauthorized Access**: Returns 401 Unauthorized or 403 Forbidden
- **Invalid Status Transition**: Returns 400 Bad Request

### Error Response Format
```json
{
  "isSuccess": false,
  "message": "Error description",
  "statusCode": 400,
  "data": null
}
```

## Performance Considerations

### Database Optimization
- Indexed on OrderNumber, BuyerId, Status, CreatedAt
- Pagination support for large result sets
- Efficient queries with proper includes for related data

### Caching Strategy
- Consider caching frequently accessed order data
- Implement cache invalidation on order updates

## Future Enhancements

### Planned Features
- **Order Notifications**: Email/SMS notifications for status changes
- **Order Tracking**: Real-time tracking integration
- **Bulk Operations**: Bulk status updates for multiple orders
- **Order Templates**: Save and reuse common order configurations
- **Advanced Analytics**: More detailed reporting and insights
- **Order Returns**: Handle return requests and processing
- **Multi-currency Support**: Support for different currencies
- **Order Splitting**: Split orders across multiple sellers

### Integration Points
- **Payment Gateway**: Integration with payment processors
- **Shipping Providers**: Integration with shipping services
- **Inventory Management**: Real-time inventory synchronization
- **Customer Service**: Integration with support systems

## Testing

### Test Coverage
- Unit tests for repository methods
- Integration tests for API endpoints
- Business logic validation tests
- Error handling tests

### Test Data
- Use the provided HTTP test file for manual testing
- Ensure proper test data setup for different scenarios
- Test edge cases like stock validation and status transitions

## Deployment Notes

### Database Migration
- Run Entity Framework migrations to create Order and OrderItem tables
- Ensure proper indexing for performance
- Consider data seeding for initial order statuses

### Configuration
- Update connection strings for production
- Configure proper authentication and authorization
- Set up logging for order operations
