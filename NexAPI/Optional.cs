using System;
using System.Collections.Generic;

namespace Nexus.NexAPI {
	public struct Optional<T> {
		private T _value;
		public Boolean HasValue { get; private set; }

		public Optional(T value) {
			this._value = value;
			this.HasValue = true;
		}

		public T Value {
			get {
				if (this.HasValue) {
					return this._value;
				}

				throw new InvalidOperationException();
			}
			set {
				if (!this.HasValue) {
					this.HasValue = true;
					this._value = value;
				}
				else {
					throw new InvalidOperationException();
				}
			}
		}

		public static implicit operator T(Optional<T> optional) {
			return optional.Value;
		}

		public static implicit operator Optional<T>(T value) {
			return new Optional<T>(value);
		}

		public static Boolean operator ==(Optional<T> optional1, Optional<T> optional2) {
			return optional1.Equals(optional2);
		}

		public static Boolean operator !=(Optional<T> optional1, Optional<T> optional2) {
			return !(optional1 == optional2);
		}

		public override Boolean Equals(Object obj) {
			if (obj is Optional<T> optional) {
				return this.Equals(optional);
			}

			return false;
		}

		public override Int32 GetHashCode() {
			unchecked {
				return (EqualityComparer<T>.Default.GetHashCode(this._value) * 397) ^ this.HasValue.GetHashCode();
			}
		}

		public Boolean Equals(Optional<T> optional) {
			if (this.HasValue && optional.HasValue) {
				return Equals(this.Value, optional.Value);
			}

			return false;
		}
	}
}