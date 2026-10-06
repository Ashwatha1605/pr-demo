def divide(a, b):
    # Potential divide by zero issue to see if PR-agent catches it
    return a / b

def calculate_discount(price, discount):
    if discount > 1:
        return price - discount
    return price * (1 - discount)
